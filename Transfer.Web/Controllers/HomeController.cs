using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using Transfer.Common;
using Transfer.Common.Settings;
using Transfer.Web.Models;

namespace Transfer.Web.Controllers;

[Authorize]
public class HomeController : BaseController
{
    public HomeController(IUnitOfWork unitOfWork, IOptions<TransferSettings> settings, ILogger<HomeController> logger, IMapper mapper)
        : base(settings, unitOfWork, logger, mapper)
    {
    }

    [AllowAnonymous]
    [Route("/")]
    public IActionResult Index()
    {
        //Logger?.LogInformation("Index called");

        //testing
        //var accounts = UnitOfWork.GetSet<DbAccount>().Count();
        //var btc = TransferSettings.TGBotToken;

        //var foo = new Foo { Id = 1, Name = "zzz" };
        //var bar = Mapper.Map<Bar>(foo);
        //var isauth = HttpContext.User.Identity.IsAuthenticated;

        return View("Index2");
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
        throw new NotImplementedException();
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
