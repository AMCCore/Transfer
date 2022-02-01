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

public class BusController : BaseController
{
    public BusController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<BusController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper) { }

    [HttpGet]
    [Route("Carrier/{carrierId}/Bus/New")]
    public async Task<IActionResult> NewBus([Required] Guid carrierId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        return View("Save", new DbBus { OrganisationId = carrierId, Organisation = entity });
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Bus/{busId}")]
    public async Task<IActionResult> BusItem([Required] Guid carrierId, [Required] Guid busId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        var bus = entity.Buses.FirstOrDefault(x => x.Id == busId);
        if (entity == null || bus == null)
            return NotFound();

        return View("Save", bus);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Save([FromForm] DbBus busModel)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == busModel.OrganisationId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            busModel.Organisation = entity;
            return View("Save", busModel);
        }
        
        if(busModel.Id.IsNullOrEmpty())
        {
            busModel.Id = Guid.NewGuid();
            busModel.Organisation = null;
            busModel.IsDeleted = false;
            await UnitOfWork.AddEntityAsync(busModel, CancellationToken.None);
        }
        else
        {
            var bus = await UnitOfWork.GetSet<DbBus>().FirstOrDefaultAsync(ss => ss.Id == busModel.Id, CancellationToken.None);
            
            if (bus.LastUpdateTick != busModel.LastUpdateTick)
                throw new InvalidOperationException();
            
            Mapper.Map(busModel, bus);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }

        return RedirectToAction(nameof(BusItem), new { carrierId = busModel.OrganisationId, busId = busModel.Id });
    }
}

