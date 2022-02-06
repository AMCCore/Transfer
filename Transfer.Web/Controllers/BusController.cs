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
using Transfer.Web.Models;
using System.Collections.Generic;
using Transfer.Bl.Dto;

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


    private async Task<BusSearchFilter> GetDataFromDb(BusSearchFilter filter = null)
    {
        filter ??= new BusSearchFilter(new List<OrganisationAssetDto>(), TransferSettings.TablePageSize);
        var query = UnitOfWork.GetSet<DbBus>().Include(x => x.Organisation).Where(x => !x.IsDeleted && !x.Organisation.IsDeleted).AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter.Model))
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Model.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filter.OrganisationName))
        {
            query = query.Where(x => x.Organisation.Name.ToLower().Contains(filter.OrganisationName.ToLower()) 
            || x.Organisation.FullName.ToLower().Contains(filter.OrganisationName.ToLower())
            || x.Organisation.INN.ToLower().Contains(filter.OrganisationName.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filter.Make))
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Make.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            query = query.Where(x => x.Organisation.City.ToLower().Contains(filter.City.ToLower()));
        }
        if (filter.Year.HasValue && filter.Year > 1930)
        {
            query = query.Where(x => x.Yaer >= filter.Year);
        }
        if (filter.PeopleCopacity.HasValue && filter.PeopleCopacity > 0)
        {
            query = query.Where(x => x.PeopleCopacity >= filter.Year);
        }

        var totalCount = await query.CountAsync(CancellationToken.None);
        var entity = await query.Skip(filter.StartRecord)
            .Take(filter.PageSize).ToListAsync(CancellationToken.None);

        filter.Results = new CommonPagedList<OrganisationAssetDto>(
            entity.Select(ss => Mapper.Map<OrganisationAssetDto>(ss)).ToList(),
            filter.PageNumber, filter.PageSize, totalCount);

        return filter;
    }

    [HttpGet]
    [Route("Bus/Search")]
    public async Task<IActionResult> Search()
    {
        var result = await GetDataFromDb();
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Bus/Search")]
    public async Task<IActionResult> SearchBus([FromForm] BusSearchFilter filter)
    {
        var result = await GetDataFromDb(filter);
        return View("Search", result);
    }

}

