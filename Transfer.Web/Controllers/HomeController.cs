using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Common;
using Transfer.Common.Security;
using Transfer.Dal.Entities;
using Transfer.Web.Models;

namespace Transfer.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork unitOfWork, IOptions<TransferSettings> settings, ILogger<HomeController> logger, IMapper mapper)
            : base(settings, unitOfWork, logger, mapper)
        {
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            Logger?.LogInformation("Index called");

            //testing
            //var accounts = UnitOfWork.GetSet<DbAccount>().Count();
            //var btc = TransferSettings.TGBotToken;

            //var foo = new Foo { Id = 1, Name = "zzz" };
            //var bar = Mapper.Map<Bar>(foo);
            var isauth = HttpContext.User.Identity.IsAuthenticated;


            return View();
        }

        [AllowAnonymous]
        public IActionResult Index2()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginModel objLoginModel = new LoginModel();
            objLoginModel.ReturnUrl = ReturnUrl;
            return View(objLoginModel);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Reg")]
        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
