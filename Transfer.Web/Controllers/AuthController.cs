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

namespace Transfer.Web.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
            : base(transferSettings, unitOfWork, logger, mapper)
        {

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel objLoginModel)
        {
            var claims = new List<Claim>();
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
