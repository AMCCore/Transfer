using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Transfer.Common;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Transfer.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Transfer.Web.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
            : base(transferSettings, unitOfWork, logger, mapper)
        {

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginModel objLoginModel = new LoginModel();
            objLoginModel.ReturnUrl = ReturnUrl;
            return View(objLoginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel objLoginModel)
        {
            //todo загрузка пользователя и проверка его прав
            var accountInfo = new { Roles = new List<Guid>(), Id = Guid.NewGuid(), OrganisationId = Guid.NewGuid() };

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(accountInfo.Roles)));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, accountInfo.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Locality, accountInfo.OrganisationId.ToString()));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = objLoginModel.RememberLogin
            });

            return LocalRedirect(string.IsNullOrWhiteSpace(objLoginModel.ReturnUrl) ? "/" : objLoginModel.ReturnUrl);
        }

        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        private static bool Authenticate(string login, string password, bool persistent)
        {
            //var _authenticationManager = GetOwinContext().Authentication;


            throw new NotImplementedException();
        }
    }
}
