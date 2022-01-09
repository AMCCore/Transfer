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
using System.Security.Claims;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Web.Models;

namespace Transfer.Web.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {
        public AuthController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<AuthController> logger, IMapper mapper)
            : base(transferSettings, unitOfWork, logger, mapper)
        {

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel objLoginModel)
        {
            //todo загрузка пользователя и проверка его прав
            var accountInfo = new { Roles = new List<Guid>(), Id = Guid.NewGuid(), OrganisationId = Guid.NewGuid() };

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(accountInfo.Roles)));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, accountInfo.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Locality, accountInfo.OrganisationId.ToString()));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, null);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(userIdentity),
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddHours(10),
                    IsPersistent = objLoginModel.RememberLogin,
                    AllowRefresh = true
                });

            return LocalRedirect(string.IsNullOrWhiteSpace(objLoginModel.ReturnUrl) ? "/" : objLoginModel.ReturnUrl);
        }
        
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}
