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

    public IActionResult About()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(About));
        return RedirectToHome();
    }

    public IActionResult Normatives()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Normatives));
        return RedirectToHome();
    }

    [Route("Contacts")]
    public IActionResult Contacts()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Contacts));
        return RedirectToHome();
    }

    [Route("Specials")]
    public IActionResult Specials()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Specials));
        return RedirectToHome();
    }

    public IActionResult Documents()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Documents));
        return RedirectToHome();
    }

    public IActionResult Vacancy()
    {
        WriteWrongWayToLog(nameof(InfoController), nameof(Vacancy));
        return RedirectToHome();
    }
}
