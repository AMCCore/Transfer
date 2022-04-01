﻿using AutoMapper;
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
using Transfer.Bl.Dto.Bus;
using Microsoft.AspNetCore.Hosting;

namespace Transfer.Web.Controllers;

public class BusController : BaseController
{
    private readonly FileController fileController;

    public BusController(IWebHostEnvironment webHostEnvironment, IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<BusController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper) 
    {
        fileController = new FileController(webHostEnvironment, transferSettings, unitOfWork, null, mapper);
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Bus/New")]
    public async Task<IActionResult> NewBus([Required] Guid carrierId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        return View("Save",  new BusDto { OrganisationId = carrierId, OrganisationName = entity.Name });
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Bus/{busId}")]
    public async Task<IActionResult> BusItem([Required] Guid carrierId, [Required] Guid busId)
    {
        var entity = await UnitOfWork.GetSet<DbBus>().Include(x => x.Organisation).Include(x => x.BusFiles).FirstOrDefaultAsync(ss => ss.OrganisationId == carrierId && ss.Id == busId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        var res = Mapper.Map<BusDto>(entity);

        return View("Save", res);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Save([FromForm] BusDto busModel)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == busModel.OrganisationId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            busModel.OrganisationName = entity.Name;
            return View("Save", busModel);
        }
        
        if(busModel.Id.IsNullOrEmpty())
        {
            busModel.Id = Guid.NewGuid();
            busModel.IsDeleted = false;
            var bus = Mapper.Map<DbBus>(busModel);
            bus.OrganisationId = busModel.OrganisationId;

            await UnitOfWork.AddEntityAsync(bus, CancellationToken.None);
        }
        else
        {
            var bus = await UnitOfWork.GetSet<DbBus>().FirstOrDefaultAsync(ss => ss.Id == busModel.Id, CancellationToken.None);
            
            if (bus.LastUpdateTick != busModel.LastUpdateTick)
                throw new InvalidOperationException();
            
            Mapper.Map(busModel, bus);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);
        }

        //Файл осаго
        await SetBusFile(busModel.Id, busModel.OsagoFileId, Common.Enums.BusFileType.Inshurance);

        //Файл СТС
        await SetBusFile(busModel.Id, busModel.RegFileId, Common.Enums.BusFileType.Reg);

        //Файл ТО
        await SetBusFile(busModel.Id, busModel.ToFileId, Common.Enums.BusFileType.TO);

        //Файл ОСГОП
        await SetBusFile(busModel.Id, busModel.OsgopFileId, Common.Enums.BusFileType.Osgop);

        //Файл Калибровка тахографа
        await SetBusFile(busModel.Id, busModel.TahografFileId, Common.Enums.BusFileType.Tahograf);

        var photos = new List<Guid>();
        if (!busModel.Photo1.IsNullOrEmpty())
            photos.Add(busModel.Photo1.Value);
        if (!busModel.Photo2.IsNullOrEmpty())
            photos.Add(busModel.Photo2.Value);
        if (!busModel.Photo3.IsNullOrEmpty())
            photos.Add(busModel.Photo3.Value);
        if (!busModel.Photo4.IsNullOrEmpty())
            photos.Add(busModel.Photo4.Value);
        if (!busModel.Photo5.IsNullOrEmpty())
            photos.Add(busModel.Photo5.Value);
        if (!busModel.Photo6.IsNullOrEmpty())
            photos.Add(busModel.Photo6.Value);

        var files = await UnitOfWork.GetSet<DbBusFile>()
            .Where(x => x.BusId == busModel.Id && !x.IsDeleted && (x.FileType == Common.Enums.BusFileType.Photo || x.FileType == Common.Enums.BusFileType.PhotoMain))
            .ToListAsync(CancellationToken.None);

        await UnitOfWork.DeleteListAsync(files, CancellationToken.None);

        var i = 0;
        foreach (var p in photos)
        {
            //avatar add
            if(i == 0)
            {
                await UnitOfWork.AddEntityAsync(new DbBusFile
                {
                    BusId = busModel.Id,
                    FileId = p,
                    IsDeleted = false,
                    FileType = Common.Enums.BusFileType.PhotoMain,
                    UploaderId = Security.CurrentAccountId
                }, CancellationToken.None);
            }
            else
            {
                await UnitOfWork.AddEntityAsync(new DbBusFile
                {
                    BusId = busModel.Id,
                    FileId = p,
                    IsDeleted = false,
                    FileType = Common.Enums.BusFileType.Photo,
                    UploaderId = Security.CurrentAccountId
                }, CancellationToken.None);
            }
            i++;
        }

        return RedirectToAction(nameof(BusItem), new { carrierId = busModel.OrganisationId, busId = busModel.Id });
    }

    private async Task SetBusFile(Guid busId, Guid? fileId, Common.Enums.BusFileType fileType)
    {
        var files = await UnitOfWork.GetSet<DbBusFile>().Where(x => x.BusId == busId && !x.IsDeleted && x.FileType == fileType).ToListAsync(CancellationToken.None);
        if (files.All(x => x.FileId != fileId))
        {
            await UnitOfWork.DeleteListAsync(files, CancellationToken.None);

            if (!fileId.IsNullOrEmpty())
            {
                await UnitOfWork.AddEntityAsync(new DbBusFile
                {
                    BusId = busId,
                    FileId = fileId.Value,
                    IsDeleted = false,
                    FileType = fileType,
                    UploaderId = Security.CurrentAccountId
                }, CancellationToken.None);
            }
        }

    }

    private async Task<BusSearchFilter> GetDataFromDb(BusSearchFilter filter = null)
    {
        filter ??= new BusSearchFilter(new List<BusSearchItem>(), TransferSettings.TablePageSize);
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
            query = query.Where(x => x.Make.ToLower().Contains(filter.Make.ToLower()));
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

        filter.Results = new CommonPagedList<BusSearchItem>(
            entity.Select(ss => Mapper.Map<BusSearchItem>(ss)).ToList(),
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
        return PartialView("SearchResults", result);
    }

}

