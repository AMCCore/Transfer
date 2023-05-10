using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Transfer.Common;
using Transfer.Common.Settings;

namespace Transfer.Web.Controllers;

[AllowAnonymous]
public class InfoController : BaseController
{
    public InfoController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<InfoController> logger, IMapper mapper)
        : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    [Route("Info")]
    public IActionResult Home()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Specials));
        return RedirectToHome();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Normatives()
    {
        return View();
    }

    
    public IActionResult Contacts()
    {
        return View();
    }

    [Route("Specials")]
    public IActionResult Specials()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Specials));
        return RedirectToHome();
    }
    
    public IActionResult Documents()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Specials));
        return RedirectToHome();
    }

    public IActionResult Vacancy()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Vacancy));
        return RedirectToHome();
    }
}
