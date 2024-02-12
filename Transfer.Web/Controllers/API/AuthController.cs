using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System;
using Transfer.Bl.Dto.API;
using Transfer.Common.Security;
using Transfer.Common;
using Transfer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Transfer.Common.Settings;
using Microsoft.Extensions.Options;
using Transfer.Web.Moduls;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Telegram.Bot.Types;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    const int passLength = 4;
    const int codeLifetime = 4;

    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityService _securityService;
    private readonly TransferSettings _transferSettings;

    public AuthController(IUnitOfWork unitOfWork, ISecurityService securityService, ITokenService tokenService, IOptions<TransferSettings> transferSettings)
    {
        _unitOfWork = unitOfWork;
        _securityService = securityService;
        _tokenService = tokenService;
        _transferSettings = transferSettings.Value;
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AllowAnonymous]
    [Route("sendAcceptCode")]
    public async Task<IActionResult> AcceptCodeSend([FromBody] SendAcceptCodeDto model, CancellationToken token = default)
    {
        try
        {
            var user = await _unitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(x => x.Phone == model.PhoneNumber, token);
            if (user == null)
            {
                return Forbid("Account not found");
            }

            var vc = user.ExternalLogins.FirstOrDefault(x => x.LoginType == Common.Enums.ExternalLoginTypeEnum.AcceptCode);

            var verificationCode = new int[passLength];
            var random = new Random();

            // generates verification code
            for (int i = 0; i < passLength; i++)
            {
                verificationCode[i] = random.Next(0, 9);
            }

            var code = string.Join(string.Empty, verificationCode);

            if (vc == null)
            {
                await _unitOfWork.AddEntityAsync(new DbExternalLogin { AccountId = user.Id, LoginType = Common.Enums.ExternalLoginTypeEnum.AcceptCode, Value = code, SubValue = DateTime.Now.AddMinutes(codeLifetime).Ticks.ToString() }, true, token);
            }
            else
            {
                vc.Value = code;
                vc.SubValue = DateTime.Now.AddMinutes(codeLifetime).Ticks.ToString();
                await _unitOfWork.UpdateAsync(vc, token);
            }

            var p = new Plusofon(_transferSettings);
            await p.FlashCallSend(user.Phone, code);


            return Ok("AcceptCode send");
        }
        catch (Exception e)
        {
#if DEBUG
            return Problem($"Exception: {e.Message}; {e.StackTrace};");
#endif
            return Problem($"Some problem {nameof(AcceptCodeSend)}");
        }
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AllowAnonymous]
    [Route("checkAcceptCode")]
    public async Task<IActionResult> AcceptCodeCheck([FromBody] CheckAcceptCodeDto model, CancellationToken token = default)
    {
        try
        {
            var user = await _unitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(x => x.Phone == model.PhoneNumber, token);
            if (user == null)
            {
                return Forbid("Account not found");
            }

            var vc = user.ExternalLogins.FirstOrDefault(x => x.LoginType == Common.Enums.ExternalLoginTypeEnum.AcceptCode);
            if (vc == null)
            {
                return Forbid("Wrong acceptc ode");
            }

            if (vc.Value == model.AcceptCode && DateTime.Now.Ticks <= Convert.ToInt64(vc.SubValue))
            {
                return Ok(JsonConvert.SerializeObject(new { authToken = _tokenService.BuildToken(user.Id) }));

            }
            return Forbid("Wrong acceptc ode");
        }
        catch (Exception e)
        {
#if DEBUG
            return Problem($"Exception: {e.Message}; {e.StackTrace};");
#endif
            return Problem($"Some problem {nameof(AcceptCodeCheck)}");
        }
    }

    /// <summary>
    /// Авторизация
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    [HttpPost]
    [Route(nameof(Login))]
    public async Task<IActionResult> Login(string login, string pass, CancellationToken token = default)
    {
        if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(pass))
        {
            var account = await _unitOfWork.GetSet<DbAccount>().Include(xx => xx.AccountRights).Where(x => x.Email == login).FirstOrDefaultAsync(token);
            if (account != null && BCrypt.Net.BCrypt.Verify(pass, account?.Password))
            {
                var generatedToken = _tokenService.BuildToken(account.Id);

                return Ok(generatedToken);
            }
        }
        return Forbid();
    }

    /// <summary>
    /// Авторизация
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    [HttpGet]
    [Route("getTempToken")]
    public async Task<IActionResult> GetTempToken([FromQuery] string AppName, CancellationToken token = default)
    {
        var claims = new[] {
                new Claim(ClaimTypes.System, AppName)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenValidator.SecKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken("MyAuthClient", "MyAuthClient", claims,
            expires: DateTime.Now.AddHours(1), signingCredentials: credentials);
        return Ok(new JwtSecurityTokenHandler().WriteToken(tokenDescriptor));
    }
}