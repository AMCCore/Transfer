using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Transfer.Common;
using Transfer.Dal.Entities;
using Transfer.Web.Models;
using Microsoft.AspNetCore.Http;
using Transfer.Common.Settings;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Extensions;

namespace Transfer.Web.Controllers;

[Authorize]
public class AuthController : BaseController
{
    public AuthController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<AuthController> logger, IMapper mapper)
        : base(transferSettings, unitOfWork, logger, mapper)
    {

    }

    [AutoValidateAntiforgeryToken]
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginModel objLoginModel)
    {
        if (!string.IsNullOrWhiteSpace(objLoginModel.UserName) && !string.IsNullOrWhiteSpace(objLoginModel.Password))
        {
            var account = await UnitOfWork.GetSet<DbAccount>().Include(xx => xx.AccountRights)
                .Where(x => !x.IsDeleted && x.Email == objLoginModel.UserName)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (BCrypt.Net.BCrypt.Verify(objLoginModel.Password, account.Password))
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(GetAccountRoles(account.AccountRights))));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()));
                //claims.Add(new Claim(ClaimTypes.Locality, account.OrganisationId.ToString()));

                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, null);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(userIdentity),
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(10),
                        IsPersistent = objLoginModel.RememberLogin,
                        AllowRefresh = true
                    });

                //return new JsonResult(new { redirect = string.IsNullOrWhiteSpace(objLoginModel.ReturnUrl) ? "/" : objLoginModel.ReturnUrl }) { StatusCode = 200 };
                return Json(new { redirect = string.IsNullOrWhiteSpace(objLoginModel.ReturnUrl) ? "/" : objLoginModel.ReturnUrl });
            }
        }

        return StatusCode(StatusCodes.Status403Forbidden, "Неверный логин или пароль");
    }

    [HttpGet]
    [Route("Logout")]
    public async Task<IActionResult> LogOut()
    {
        Security.CurrentUserClearCache();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return LocalRedirect("/");
    }

    /// <summary>
    /// Группируем права из БД по организациям
    /// </summary>
    private static Dictionary<Guid, List<Guid>> GetAccountRoles(IEnumerable<DbAccountRight> roles)
    {
        if (roles == null || !roles.Any())
        {
            return new Dictionary<Guid, List<Guid>>();
        }

        var res = roles.GroupBy(sr => sr.OrganisationId ?? Guid.Empty)
            .ToDictionary(g => g.Key,
                g => g.Where(x => !x.Right.IsDeleted).Select(ss => ss.RightId.Value).ToList());

        return res;
    }

    [HttpGet]
    [Route("TgAccept/{userId}")]
    public async Task<IActionResult> TgAccept(Guid userId)
    {
        var account = await UnitOfWork.GetSet<DbAccount>().Include(xx => xx.AccountRights)
            .Where(x => !x.IsDeleted && x.Id == userId).FirstOrDefaultAsync(CancellationToken.None);

        if(!account.AccountRights.Any(x => x.RightId == AccountAccessRights.TelegramBotUsage.GetEnumGuid()))
        {
            await UnitOfWork.AddEntityAsync(new DbAccountRight { AccountId = userId, RightId = AccountAccessRights.TelegramBotUsage.GetEnumGuid() });
        }

        return Redirect("/");
    }
}
