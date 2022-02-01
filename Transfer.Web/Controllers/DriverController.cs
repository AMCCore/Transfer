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

        return View("Save", new DbDriver { OrganisationId = carrierId, Organisation = entity });
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Driver/{driverId}")]
    public async Task<IActionResult> DriverItem([Required] Guid carrierId, [Required] Guid driverId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        var bus = entity.Drivers.FirstOrDefault(x => x.Id == driverId);
        if (entity == null || bus == null)
            return NotFound();

        return View("Save", bus);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Save([FromForm] DbDriver driverModel)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == driverModel.OrganisationId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            driverModel.Organisation = entity;
            return View("Save", driverModel);
        }

        if (driverModel.Id.IsNullOrEmpty())
        {
            driverModel.Id = Guid.NewGuid();
            driverModel.Organisation = null;
            driverModel.IsDeleted = false;
            await UnitOfWork.AddEntityAsync(driverModel, CancellationToken.None);
        }
        else
        {
            var driver = await UnitOfWork.GetSet<DbDriver>().FirstOrDefaultAsync(ss => ss.Id == driverModel.Id, CancellationToken.None);

            if (driver.LastUpdateTick != driverModel.LastUpdateTick)
                throw new InvalidOperationException();

            Mapper.Map(driverModel, driver);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }


        return RedirectToAction(nameof(DriverItem), new { carrierId = driverModel.OrganisationId, driverId = driverModel.Id });
    }
}
