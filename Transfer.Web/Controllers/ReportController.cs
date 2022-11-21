using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Transfer.Common.Settings;
using Transfer.Common;
using Transfer.Bl.Dto.Report;
using Transfer.Common.Extensions;

namespace Transfer.Web.Controllers;

[Authorize]
[Route("Reports")]
public class ReportController : BaseController
{
    public ReportController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<ReportController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    [Route("List")]
    public IActionResult Index()
    {
        return View("List");
    }

    [Route("DataInput")]
    public IActionResult DataInput()
    {
        return View("BaseReport", new BaseReportDto { DateFrom = DateTime.Now.AddDays(-7), DateTo = DateTime.Now, Name = "Отчет о внесении перевозчиков", Action = "DataInputGen" });
    }

    [Route("DataInputGen")]
    public IActionResult DataInputGen([FromForm] BaseReportDto model)
    {
        return NotFound();
    }

}
