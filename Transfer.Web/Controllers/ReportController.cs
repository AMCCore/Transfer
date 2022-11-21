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
using AspNetCore;
using Transfer.Dal.Entities;
using System.Linq;

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

    [HttpGet]
    [Route("DataInput")]
    public IActionResult DataInput()
    {
        return View("DataInput", new BaseReportDto<InputDataReportDto> { DateFrom = DateTime.Now.AddDays(-7), DateTo = DateTime.Now, Name = "Отчет о внесении перевозчиков", Action = "DataInputGen" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("DataInputGen")]
    public IActionResult DataInputGen([FromForm] BaseReportDto<InputDataReportDto> model)
    {
        var _res = UnitOfWork.GetSet<DbBus>().Where(x => !x.IsDeleted);

        if(model.AsFile)
        {
            return View("DataInput", model);
        }
        else
        {
            return View("DataInput", model);
        }
    }

}
