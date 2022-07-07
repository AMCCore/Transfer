using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Dal.Entities;
using Transfer.Common.Extensions;
using Transfer.Bl.Dto.Driver;
using Transfer.Common.Settings;
using Transfer.Web.Moduls;

namespace Transfer.Web.Controllers;

public class DriverController : BaseController
{
    public DriverController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<DriverController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper) { }

    [HttpGet]
    [Route("Carrier/{carrierId}/Driver/New")]
    public async Task<IActionResult> NewDriver([Required] Guid carrierId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        return View("Save", new DriverDto { OrganisationId = carrierId, OrganisationName = entity.Name });
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Driver/{driverId}")]
    public async Task<IActionResult> DriverItem([Required] Guid carrierId, [Required] Guid driverId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        var bus = entity.Drivers.FirstOrDefault(x => x.Id == driverId);
        if (entity == null || bus == null)
            return NotFound();

        return View("Save", Mapper.Map<DriverDto>(bus));
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Save([FromForm] DriverDto driverModel)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == driverModel.OrganisationId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("Save", driverModel);
        }

        if (driverModel.Id.IsNullOrEmpty())
        {
            driverModel.Id = Guid.NewGuid();
            driverModel.IsDeleted = false;
            var driver = Mapper.Map<DbDriver>(driverModel);
            driver.OrganisationId = driverModel.OrganisationId;
            await UnitOfWork.AddEntityAsync(driver, CancellationToken.None);
        }
        else
        {
            var driver = await UnitOfWork.GetSet<DbDriver>().FirstOrDefaultAsync(ss => ss.Id == driverModel.Id, CancellationToken.None);

            if (driver.LastUpdateTick != driverModel.LastUpdateTick)
                throw new InvalidOperationException();

            Mapper.Map(driverModel, driver);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }

        //права
        await SetDriverFile(driverModel.Id, driverModel.License1.Value, Common.Enums.DriverFileType.License);

        //права обр ст
        await SetDriverFile(driverModel.Id, driverModel.License2.Value, Common.Enums.DriverFileType.LicenseBack);

        //тахограф
        await SetDriverFile(driverModel.Id, driverModel.TahografFileId.Value, Common.Enums.DriverFileType.TahografCard);

        //аватар
        await SetDriverFile(driverModel.Id, driverModel.Avatar.Value, Common.Enums.DriverFileType.Avatar);



        return RedirectToAction(nameof(DriverItem), new { carrierId = driverModel.OrganisationId, driverId = driverModel.Id });
    }

    private async Task SetDriverFile(Guid driverId, Guid fileId, Common.Enums.DriverFileType fileType)
    {
        var files = await UnitOfWork.GetSet<DbDriverFile>().Where(x => x.DriverId == driverId && !x.IsDeleted && x.FileType == fileType).ToListAsync(CancellationToken.None);
        if (files.All(x => x.FileId != fileId))
        {
            await UnitOfWork.DeleteListAsync(files, CancellationToken.None);

            await UnitOfWork.AddEntityAsync(new DbDriverFile
            {
                FileId = fileId,
                DriverId = driverId,
                IsDeleted = false,
                FileType = fileType,
                UploaderId = Security.CurrentAccountId
            }, CancellationToken.None);
        }

    }
}
