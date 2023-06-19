using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Transfer.Bl.Dto.Report;
using Transfer.Common;
using Transfer.Common.Enums;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Extensions;
using Transfer.Common.Settings;
using Transfer.Dal.Entities;
using Transfer.Web.Moduls;

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
        if(!Security.HasRightForSomeOrganisation(ReportAccessRights.DataInputReport))
        {
            return Forbid();
        }

        return View("DataInput", new BaseReportDto<InputDataReportDto> { DateFrom = DateTime.Now.AddDays(-7), DateTo = DateTime.Now, Name = "Отчет о внесении перевозчиков", Action = "DataInputGen" });
    }

    /// <summary>
    /// Отчет о внесении перевозчиков
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("DataInputGen")]
    public async Task<IActionResult> DataInputGen([FromForm] BaseReportDto<InputDataReportDto> model)
    {
        if (!Security.HasRightForSomeOrganisation(ReportAccessRights.DataInputReport))
        {
            return Forbid();
        }

        var q = UnitOfWork.GetSet<DbBus>().Where(x => !x.IsDeleted).AsQueryable();
        if(model.DateFrom.HasValue)
        {
            q = q.Where(x => x.DateCreated >= model.DateFrom);
        }
        if(model.DateTo.HasValue)
        {
            q = q.Where(x => x.DateCreated < model.DateTo.Value.Date.AddDays(1));
        }

        var res = await q.OrderByDescending(x => x.DateCreated).Select(a => new InputDataReportDto {
            DateInput = a.DateCreated,
            OSAGOToDate = null,
            OSGOPToDate = null, 
            TOToDate = a.ToDate,
            Carrier = a.Organisation != null ? a.Organisation.Name : null,
            LicenseNumber = a.LicenseNumber,
            Make = a.Make,
            Model = a.Model,
            PeopleCopacity = a.PeopleCopacity,
            Yaer = a.Yaer,
            Reg = $"{a.RegSeries} {a.RegNumber}",
            Manager = a.Creator != null ? $"{a.Creator.PersonData.LastName} {a.Creator.PersonData.FirstName} {a.Creator.PersonData.MiddleName}" : null,
            FotoCount = a.BusFiles.Count(x => !x.IsDeleted && x.FileType == Common.Enums.BusFileTypeEnum.Photo)
        }).ToListAsync();
        model.Results = res;


        if(model.AsFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Отчет о внесении перевозчиков");
            worksheet.Cells[1, 1].Value = "Дата (внесения данных)";
            worksheet.Cells[1, 2].Value = "Пользователь";
            worksheet.Cells[1, 3].Value = "Перевозчик";
            worksheet.Cells[1, 4].Value = "ТС марка";
            worksheet.Cells[1, 5].Value = "ТС модель";
            worksheet.Cells[1, 6].Value = "Год";
            worksheet.Cells[1, 7].Value = "Гос. номер";
            worksheet.Cells[1, 8].Value = "Посад. мест";
            worksheet.Cells[1, 9].Value = "СТС";
            worksheet.Cells[1, 10].Value = "ОСАГО";
            worksheet.Cells[1, 11].Value = "ОСГОП";
            worksheet.Cells[1, 12].Value = "Диагност. Карта";
            worksheet.Cells[1, 13].Value = "Фото";

            worksheet.Cells[1, 1, 1, 13].Style.Font.Bold = true;
            worksheet.Cells[1, 1, 1, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


            var i = 2;

            foreach(var z in res)
            {
                worksheet.Cells[i, 1].Value = z.DateInput.Date;
                worksheet.Cells[i, 1].Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Cells[i, 2].Value = z.Manager;
                worksheet.Cells[i, 3].Value = z.Carrier;
                worksheet.Cells[i, 4].Value = z.Make;
                worksheet.Cells[i, 5].Value = z.Model;
                worksheet.Cells[i, 6].Value = z.Yaer;
                worksheet.Cells[i, 7].Value = z.LicenseNumber;
                worksheet.Cells[i, 8].Value = z.PeopleCopacity;
                worksheet.Cells[i, 9].Value = z.Reg;
                worksheet.Cells[i, 10].Value = z.OSAGOToDate?.Date;
                worksheet.Cells[i, 10].Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Cells[i, 11].Value = z.OSGOPToDate?.Date;
                worksheet.Cells[i, 11].Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Cells[i, 12].Value = z.TOToDate?.Date;
                worksheet.Cells[i, 12].Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Cells[i, 13].Value = z.FotoCount;

                //worksheet.Cells[i, 1, i, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                i++;
            }

            var fileStream = new MemoryStream();
            await package.SaveAsAsync(fileStream);
            fileStream.Position = 0;

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }
        else
        {
            return View("DataInput", model);
        }
    }


    [HttpGet]
    [Route("TripRequests")]
    public IActionResult TripRequests()
    {
        if (!Security.HasRightForSomeOrganisation(ReportAccessRights.DataInputReport))
        {
            return Forbid();
        }

        return View("TripRequests", new BaseReportDto<TripRequestsReportDto> { DateFrom = DateTime.Now.AddDays(-7), DateTo = DateTime.Now, Name = "Отчет о внесении запросов на перевозки", Action = "TripRequestsGen" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("TripRequestsGen")]
    public async Task<IActionResult> TripRequestsGen([FromForm] BaseReportDto<TripRequestsReportDto> model)
    {
        if (!Security.HasRightForSomeOrganisation(ReportAccessRights.TripRequestReport))
        {
            return Forbid();
        }

        var q = UnitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted).AsQueryable();
        if (model.DateFrom.HasValue)
        {
            q = q.Where(x => x.DateCreated >= model.DateFrom);
        }
        if (model.DateTo.HasValue)
        {
            q = q.Where(x => x.DateCreated < model.DateTo.Value.Date.AddDays(1));
        }

        var q3 = UnitOfWork.GetSet<DbAccount>().AsQueryable();
        var q2 = UnitOfWork.GetSet<DbHistoryLog>().OrderBy(x => x.DateCreated).AsQueryable();

        var res = await q.OrderByDescending(x => x.DateCreated).Select(a => new TripRequestsReportDto
        {
            DateInput = a.DateCreated,
            DateStart = a.TripDate,
            AddressFinish = a.AddressTo,
            AddressStart = a.AddressFrom,
            ChildTrip = a.TripOptions.Any(x => x.TripOptionId == TripOptionsEnum.ChildTrip.GetEnumGuid()),
            Identifier = a.Identifiers.OrderByDescending(x => x.LastUpdateTick).Select(x => x.Identifier).First(),
            PeopleCopacity = a.Passengers,
            OfferCount = a.TripRequestOffers.Count(x => !x.IsDeleted),
            TripRequestId = a.Id,
            State = a.StateEnum,
            Offer = a.TripRequestOffers.Where(x => !x.IsDeleted && x.Chosen).Select(x => x.Amount).FirstOrDefault(),
            Manager = q3.Where(y => y.Id == q2.Where(x => x.EntityId == a.Id).Select(x => x.AccountId).FirstOrDefault()).Select(y => $"{y.PersonData.LastName} {y.PersonData.FirstName} {y.PersonData.MiddleName}").FirstOrDefault(),
            Requester = a.СhartererName,
        }).ToListAsync();
        model.Results = res;

        if(model.AsFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Отчет о внесении запросов на перевозки");
            worksheet.Cells[1, 1].Value = "Дата (внесения данных)";
            worksheet.Cells[1, 2].Value = "Заказ №";
            worksheet.Cells[1, 3].Value = "Заказчик";
            worksheet.Cells[1, 4].Value = "Дата и время подачи";
            worksheet.Cells[1, 5].Value = "Начальный пункт маршрута";
            worksheet.Cells[1, 6].Value = "Конечный пункт маршрута";
            worksheet.Cells[1, 7].Value = "Стоимость, руб";
            worksheet.Cells[1, 8].Value = "Посадочных мест";
            worksheet.Cells[1, 9].Value = "Перевозка детей";
            worksheet.Cells[1, 10].Value = "Отклики";
            worksheet.Cells[1, 11].Value = "Статус";
            worksheet.Cells[1, 12].Value = "Пользователь";

            worksheet.Cells[1, 1, 1, 12].Style.Font.Bold = true;
            worksheet.Cells[1, 1, 1, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            var i = 2;

            foreach (var z in res)
            {
                worksheet.Cells[i, 1].Value = z.DateInput.Date;
                worksheet.Cells[i, 1].Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Cells[i, 2].Value = z.Identifier;
                worksheet.Cells[i, 3].Value = z.Requester;
                worksheet.Cells[i, 4].Value = z.DateStart;
                worksheet.Cells[i, 4].Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Cells[i, 5].Value = z.AddressStart;
                worksheet.Cells[i, 6].Value = z.AddressFinish;
                worksheet.Cells[i, 7].Value = z.Offer > 0 ? z.Offer : 0;
                worksheet.Cells[i, 8].Value = z.PeopleCopacity;
                worksheet.Cells[i, 9].Value = z.ChildTrip ? "да" : "нет";
                worksheet.Cells[i, 10].Value = z.OfferCount;
                worksheet.Cells[i, 11].Value = z.StateName;
                worksheet.Cells[i, 12].Value = z.Manager;

                i++;
            }

            var fileStream = new MemoryStream();
            await package.SaveAsAsync(fileStream);
            fileStream.Position = 0;

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }



        return View("TripRequests", model);
    }
}
