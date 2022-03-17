﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Transfer.Bl.Dto.TripRequest;
using Transfer.Common;
using Transfer.Dal.Entities;
using Transfer.Web.Models;
using Transfer.Web.Models.TripRequest;
using Transfer.Common.Enums;
using Transfer.Common.Extensions;
using Transfer.Bl.Dto.Driver;

namespace Transfer.Web.Controllers;

[Authorize]
public class TripRequestController : BaseController
{
    public TripRequestController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripRequestController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    private async Task<RequestSearchFilter> GetDataFromDb(RequestSearchFilter filter = null)
    {
        filter ??= new RequestSearchFilter(new List<TripRequestSearchResultItem>(), TransferSettings.TablePageSize);
        var query = UnitOfWork.GetSet<DbTripRequest>().Include(x => x.Charterer).Where(x => !x.IsDeleted).OrderBy(x => x.DateCreated).AsQueryable();

        if (filter.OrderByName)
        {
            query = query.OrderBy(x => x.СhartererName).ThenBy(x => x.Charterer.Name);
        }

        if (filter.OrderByRating)
        {
            //query = query.OrderBy(x => x.Rating).ThenBy(x => x.Name);
        }

        if (filter.OrderByChecked)
        {
            //query = query.OrderBy(x => x.Rating).ThenBy(x => x.Name);
        }

        if (filter.OrderByChild)
        {
            query = query.OrderByDescending(x => x.TripOptions.Any(y => y.TripOptionId == TripOptions.ChildTrip.GetEnumGuid()))
                .ThenBy(x => x.СhartererName)
                .ThenBy(x => x.Charterer.Name);
        }

        var totalCount = await query.CountAsync(CancellationToken.None);
        var entity = await query.Skip(filter.StartRecord)
            .Take(filter.PageSize).ToListAsync(CancellationToken.None);

        filter.Results = new CommonPagedList<TripRequestSearchResultItem>(
            entity.Select(ss => Mapper.Map<TripRequestSearchResultItem>(ss)).ToList(),
            filter.PageNumber, filter.PageSize, totalCount);

        return filter;
    }

    [Route("TripRequests")]
    [HttpGet]
    public async Task<IActionResult> Search()
    {
        var result = await GetDataFromDb();
        return View(result);
    }

    [HttpPost]
    [Route("TripRequests")]
    public async Task<IActionResult> Search(RequestSearchFilter filter)
    {
        var result = await GetDataFromDb(filter);

        return PartialView("SearchResults", result);
    }

    [HttpGet]
    [Route("TripRequest/{requestId}")]
    public async Task<IActionResult> TripRequest(Guid requestId)
    {
        var entity = await UnitOfWork.GetSet<DbTripRequest>().Include(x => x.Charterer).Include(x => x.TripOptions).FirstOrDefaultAsync(ss => ss.Id == requestId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        var model = Mapper.Map<TripRequestDto>(entity);
        return View("Save", model);
    }

    [HttpGet]
    [Route("TripRequest/New")]
    public IActionResult NewTripRequest()
    {
        return View("Save", new TripRequestDto
        {
            TripDate = DateTime.Now.AddDays(1).ChangeTime(9, 0),
            PaymentType = (int)PaymentType.Card
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Save([FromForm] TripRequestDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("Save", model);
        }

        if (model.Id.IsNullOrEmpty())
        {
            model.Id = Guid.NewGuid();
            var entity = Mapper.Map<DbTripRequest>(model);
            await UnitOfWork.AddEntityAsync(entity, CancellationToken.None);
            await SetTripOptions(entity, model);
        }
        else
        {
            var entity = await UnitOfWork.GetSet<DbTripRequest>().FirstOrDefaultAsync(ss => ss.Id == model.Id, CancellationToken.None);

            if (entity.LastUpdateTick != model.LastUpdateTick)
                throw new InvalidOperationException();

            Mapper.Map(model, entity);
            await SetTripOptions(entity, model);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }

        return RedirectToAction(nameof(TripRequest), new { requestId = model.Id });
    }

    private async Task SetTripOptions(DbTripRequest entity, TripRequestDto model)
    {
        foreach(var to in await UnitOfWork.GetSet<DbTripRequestOption>().Where(x => x.TripRequestId == entity.Id).ToListAsync(CancellationToken.None))
        {
            await UnitOfWork.DeleteAsync(to, CancellationToken.None);
        }
        if(model.ChildTrip)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption { 
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptions.ChildTrip.GetEnumGuid()
            }, CancellationToken.None);
        }
        if (model.StandTrip)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptions.IdleTrip.GetEnumGuid()
            }, CancellationToken.None);
        }
        if (model.PaymentType == (int)PaymentType.Card)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptions.CardPayment.GetEnumGuid()
            }, CancellationToken.None);

        }
        else if (model.PaymentType == (int)PaymentType.Cash)
        {
            await UnitOfWork.AddEntityAsync(new DbTripRequestOption
            {
                Id = Guid.NewGuid(),
                TripRequestId = entity.Id,
                TripOptionId = TripOptions.CashPayment.GetEnumGuid()
            }, CancellationToken.None);
        }
    }

}