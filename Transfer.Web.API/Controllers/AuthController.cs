using Microsoft.AspNetCore.Mvc;
using Transfer.Common.Security;
using Transfer.Common;
using Transfer.Bl.Dto;
using Microsoft.AspNetCore.Authorization;
using Transfer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Transfer.Bl.Dto.API;
using Transfer.Common.Extensions;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Transfer.Web.API.Controllers;

[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    const int passLength = 4;
    const int codeLifetime = 4;

    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityService _securityService;

    public AuthController(IUnitOfWork unitOfWork, ISecurityService securityService)
    {
        _unitOfWork = unitOfWork;
        _securityService = securityService;
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AllowAnonymous]
    [Route("sendAcceptCode")]
    public async Task<IActionResult> SendAcceptCode([FromBody] SendAcceptCodeDto model, CancellationToken token = default)
    {
        try
        {
            var user = await _unitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(x => x.Phone == model.PhoneNumber, token);
            if (user == null)
            {
                return Forbid("Account not found");
            }

            var vc = user.ExternalLogins.FirstOrDefault(x => x.LoginType == Common.Enums.ExternalLoginTypeEnum.AcceptCode);

            var verificationCode = new int[4];
            var random = new Random();

            // generates verification code
            for (int i = 0; i < passLength; i++)
            {
                verificationCode[i] = random.Next(0, 9);
            }

            var code = string.Join("", verificationCode);

            if(vc == null)
            {
                await _unitOfWork.AddEntityAsync(new DbExternalLogin { LoginType = Common.Enums.ExternalLoginTypeEnum.AcceptCode, Value = code, SubValue = DateTime.Now.AddMinutes(codeLifetime).Ticks.ToString() }, true, token);
            }
            else
            {
                vc.Value = code;
                vc.SubValue = DateTime.Now.AddMinutes(codeLifetime).Ticks.ToString();
                await _unitOfWork.UpdateAsync(vc, token);
            }

            //-- взаимодействие с API отправки СМС


            return Ok("AcceptCode send");
        }
        catch (Exception e)
        {
#if DEBUG
            return Problem($"Exception: {e.Message}; {e.StackTrace};");
#endif
            return Problem($"Some problem {nameof(SendAcceptCode)}");
        }
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AllowAnonymous]
    [Route("checkAcceptCode")]
    public async Task<IActionResult> CheckAcceptCode([FromBody] CheckAcceptCodeDto model, CancellationToken token = default)
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
            return Problem($"Some problem {nameof(CheckAcceptCode)}");
        }
    }
}
