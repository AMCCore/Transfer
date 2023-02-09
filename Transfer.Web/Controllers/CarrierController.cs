using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Transfer.Bl.Dto.Carrier;
using Transfer.Common;
using Transfer.Dal.Entities;
using Transfer.Web.Models;
using Transfer.Web.Models.Carrier;
using Transfer.Bl.Dto;
using Transfer.Common.Extensions;
using Transfer.Common.Settings;
using Transfer.Web.Moduls;

namespace Transfer.Web.Controllers;

[Authorize]
public class CarrierController : BaseController
{
    public CarrierController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork,
        ILogger<CarrierController> logger, IMapper mapper)
        : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    private async Task<CarrierSearchFilter> GetDataFromDb(CarrierSearchFilter filter = null)
    {
        filter ??= new CarrierSearchFilter(new List<CarrierSearchResultItem>(), TransferSettings.TablePageSize);
        var query = UnitOfWork.GetSet<DbOrganisation>().Where(x => !x.IsDeleted).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Inn))
        {
            query = query.Where(x => x.INN.ToLower().Contains(filter.Inn.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            query = query.Where(x => x.Address.ToLower().Contains(filter.City.ToLower()) || x.FactAddress.ToLower().Contains(filter.City.ToLower()) || x.City.ToLower().Contains(filter.City.ToLower()));
        }


        if (filter.OrderByName)
        {
            query = query.OrderBy(x => x.Name);
        }

        if (filter.OrderByRating)
        {
            query = query.OrderBy(x => x.Rating).ThenBy(x => x.Name);
        }

        if (filter.OrderByChecked)
        {
            query = query.OrderBy(x => x.Checked).ThenBy(x => x.Name);
        }

        var totalCount = await query.CountAsync(CancellationToken.None);

        var entity = await query.Skip(filter.StartRecord)
            .Take(filter.PageSize).ToListAsync(CancellationToken.None);

        filter.Results = new CommonPagedList<CarrierSearchResultItem>(
            entity.Select(ss => Mapper.Map<CarrierSearchResultItem>(ss)).ToList(),
            filter.PageNumber, filter.PageSize, totalCount);

        return filter;
    }

    [HttpGet]
    [Route("Carrier/Search")]
    public async Task<IActionResult> Search()
    {
        var result = await GetDataFromDb();

        return View(result);
    }

    [HttpPost]
    [Route("Carrier/Search")]
    public async Task<IActionResult> SearchCarrier(CarrierSearchFilter filter)
    {
        var result = await GetDataFromDb(filter);

        return PartialView("SearchResults", result);
    }

    [HttpGet]
    [Route("Carrier/New")]
    public async Task<IActionResult> NewCarrier()
    {
        ViewBag.Regions = await GetRegionsAsync();
        return View("CarrierEdit", new CarrierDto());
    }

    [HttpGet]
    [Route("Carrier/{carrierId}")]
    public async Task<IActionResult> CarrierItem(Guid carrierId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (entity != null)
        {
            var res = Mapper.Map<CarrierDto>(entity);
            Mapper.Map(entity.BankDetails.FirstOrDefault(x => !x.IsDeleted), res);

            return View("Carrier", res);
        }
        return NotFound();
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Edit")]
    public async Task<IActionResult> CarrierItemEdit(Guid carrierId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (entity != null)
        {
            var res = Mapper.Map<CarrierDto>(entity);
            Mapper.Map(entity.BankDetails.FirstOrDefault(x => !x.IsDeleted), res);

            ViewBag.Regions = await GetRegionsAsync();
            return View("CarrierEdit", res);
        }
        return NotFound();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [Route("Carrier/Save")]
    public async Task<IActionResult> Save([FromForm] CarrierDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            ViewBag.Regions = await GetRegionsAsync();
            return View("CarrierEdit", model);
        }

        if (!model.Agreement)
        {
            ViewBag.ErrorMsg = "Ошибка согласия перс данных";
            ViewBag.Regions = await GetRegionsAsync();
            return View("CarrierEdit", model);
        }

        if (!string.IsNullOrWhiteSpace(model.INN) && await UnitOfWork.GetSet<DbOrganisation>().AnyAsync(x => x.Id != model.Id && !x.IsDeleted && x.INN.ToLower() == model.INN.ToLower()))
        {
            ViewBag.ErrorMsg = "Организация с таким ИНН уже существует";
            ViewBag.Regions = await GetRegionsAsync();
            return View("CarrierEdit", model);
        }

        await UnitOfWork.BeginTransactionAsync();

        if (model.Id.IsNullOrEmpty())
        {
            var org = Mapper.Map<DbOrganisation>(model);
            org.Id = Guid.NewGuid();
            org.CreatorId = Security.CurrentAccountId;
            model.Id = org.Id;
            //временно разрешить организации без email
            if (string.IsNullOrWhiteSpace(org.Email))
            {
                org.Email = " ";
            }
            //временно разрешить организации без директора
            if (string.IsNullOrWhiteSpace(org.DirectorFio))
            {
                org.DirectorFio = " ";
            }
            if (string.IsNullOrWhiteSpace(org.DirectorPosition))
            {
                org.DirectorPosition = " ";
            }


            await UnitOfWork.AddEntityAsync(org, CancellationToken.None);
            var br = Mapper.Map<DbBankDetails>(model);
            br.Id = Guid.NewGuid();
            br.OrganisationId = org.Id;
            await UnitOfWork.AddEntityAsync(br, CancellationToken.None);
        }
        else
        {
            var org = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.Id);
            if (org == null)
                throw new KeyNotFoundException();
            Mapper.Map(model, org);

            //временно разрешить организации без email
            if (string.IsNullOrWhiteSpace(org.Email))
            {
                org.Email = " ";
            }

            //временно разрешить организации без директора
            if (string.IsNullOrWhiteSpace(org.DirectorFio))
            {
                org.DirectorFio = " ";
            }
            if (string.IsNullOrWhiteSpace(org.DirectorPosition))
            {
                org.DirectorPosition = " ";
            }

            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            var brd = await UnitOfWork.GetSet<DbBankDetails>().Where(x => !x.IsDeleted && x.OrganisationId == org.Id).ToListAsync(CancellationToken.None);
            foreach (var b in brd)
            {
                b.IsDeleted = true;
            }
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            var br = Mapper.Map<DbBankDetails>(model);
            br.Id = Guid.NewGuid();
            br.OrganisationId = org.Id;
            await UnitOfWork.AddEntityAsync(br, CancellationToken.None);
        }

        //лтцензия
        await SetCarrierFile(model.Id, model.LicenceFileId, Common.Enums.OrganisationFileTypeEnum.License);

        //логотип
        await SetCarrierFile(model.Id, model.LogoFileId, Common.Enums.OrganisationFileTypeEnum.Logo);

        //регионы работы
        await SetCarrierWorkingArea(model.Id, model.WorkingAreas);

        await UnitOfWork.CommitAsync();

        return RedirectToAction(nameof(CarrierItem), new { carrierId = model.Id });
    }

    internal static async Task<OrganisationAssetsSearchFilter> GetAssetsDataFromDb(IUnitOfWork UnitOfWork, IMapper Mapper, TransferSettings TransferSettings, Guid OrganisationId)
    {
        return await GetAssetsDataFromDb(UnitOfWork, Mapper, TransferSettings, new OrganisationAssetsSearchFilter(new List<OrganisationAssetDto>(), TransferSettings.TablePageSize)
        {
            OrganisationId = OrganisationId
        });
    }

    internal static async Task<OrganisationAssetsSearchFilter> GetAssetsDataFromDb(IUnitOfWork UnitOfWork, IMapper Mapper, TransferSettings TransferSettings, OrganisationAssetsSearchFilter filter = null)
    {
        filter ??= new OrganisationAssetsSearchFilter(new List<OrganisationAssetDto>(), TransferSettings.TablePageSize);
        var totalCount = 0;
        var entitys = new List<OrganisationAssetDto>();

        if (filter.AssetType == OrganisationAssetType.Driver)
        {
            var query = UnitOfWork.GetSet<DbOrganisation>().Where(x => !x.IsDeleted && x.Id == filter.OrganisationId).SelectMany(x => x.Drivers);
            totalCount = await query.CountAsync(CancellationToken.None);
            var entity = await query.Skip(filter.StartRecord).Take(filter.PageSize).ToListAsync(CancellationToken.None);
            entitys = entity.Select(ss => Mapper.Map<OrganisationAssetDto>(ss)).ToList();
        }
        else if (filter.AssetType == OrganisationAssetType.Bus)
        {
            var query = UnitOfWork.GetSet<DbOrganisation>().Where(x => !x.IsDeleted && x.Id == filter.OrganisationId).SelectMany(x => x.Buses);
            totalCount = await query.CountAsync(CancellationToken.None);
            var entity = await query.Skip(filter.StartRecord).Take(filter.PageSize).ToListAsync(CancellationToken.None);
            entitys = entity.Select(ss => Mapper.Map<OrganisationAssetDto>(ss)).ToList();
        }
        else if (filter.AssetType == OrganisationAssetType.User)
        {
            var query = UnitOfWork.GetSet<DbOrganisation>().Where(x => !x.IsDeleted && x.Id == filter.OrganisationId).SelectMany(x => x.Accounts).Where(x => !x.Account.IsDeleted).Select(x => x.Account);
            totalCount = await query.CountAsync(CancellationToken.None);
            var entity = await query.Skip(filter.StartRecord).Take(filter.PageSize).ToListAsync(CancellationToken.None);
            entitys = entity.Select(ss => Mapper.Map<OrganisationAssetDto>(ss)).ToList();
        }

        filter.Results = new CommonPagedList<OrganisationAssetDto>(entitys, filter.PageNumber, filter.PageSize, totalCount);

        return filter;
    }

    public async Task<IActionResult> OrganisationAssets(Guid organisationId)
    {
        var result = await CarrierController.GetAssetsDataFromDb(UnitOfWork, Mapper, TransferSettings, organisationId);

        return PartialView("Assets", result);
    }

    [HttpPost]
    public async Task<IActionResult> SearchCarrierAssets(OrganisationAssetsSearchFilter filter)
    {
        var result = await GetAssetsDataFromDb(UnitOfWork, Mapper, TransferSettings, filter);

        return PartialView("/Views/Carrier/Assets.cshtml", result);
    }

    private async Task SetCarrierFile(Guid carrierId, Guid? fileId, Common.Enums.OrganisationFileTypeEnum fileType)
    {
        var files = await UnitOfWork.GetSet<DbOrganisationFile>().Where(x => x.OrganisationId == carrierId && !x.IsDeleted && x.FileType == fileType).ToListAsync(CancellationToken.None);
        if (files.All(x => x.FileId != fileId))
        {
            await UnitOfWork.DeleteListAsync(files, CancellationToken.None);

            if (!fileId.IsNullOrEmpty())
            {
                await UnitOfWork.AddEntityAsync(new DbOrganisationFile
                {
                    OrganisationId = carrierId,
                    FileId = fileId.Value,
                    IsDeleted = false,
                    FileType = fileType,
                    UploaderId = Security.CurrentAccountId
                }, CancellationToken.None);

            }
        }

    }

    private async Task<IDictionary<Guid, string>> GetRegionsAsync(params Guid[] exisitingRegs)
    {
        return await UnitOfWork.GetSet<DbRegion>().ToDictionaryAsync(x => x.Id, y => y.Name, CancellationToken.None);
    }

    private async Task SetCarrierWorkingArea(Guid carrierId, params string[] regions)
    {
        var eRegions = await UnitOfWork.GetSet<DbOrganisationWorkingArea>().Where(x => x.OrganisationId == carrierId).ToListAsync(CancellationToken.None);
        await UnitOfWork.DeleteListAsync(eRegions, CancellationToken.None);

        foreach (var region in regions)
        {
            if (Guid.TryParse(region, out var regId) && UnitOfWork.GetSet<DbRegion>().Any(x => x.Id == regId))
            {
                await UnitOfWork.AddEntityAsync(new DbOrganisationWorkingArea { 
                    RegionId = regId,
                    OrganisationId = carrierId
                });
            }
        }
    }
}

public class CarrierAssetsViewComponent : ViewComponent
{
    protected readonly ILogger Logger;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly TransferSettings TransferSettings;
    protected readonly IMapper Mapper;

    public CarrierAssetsViewComponent(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork,
        ILogger<CarrierController> logger, IMapper mapper)
    {
        TransferSettings = transferSettings.Value;
        UnitOfWork = unitOfWork;
        Logger = logger;
        Mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid organisationId)
    {
        var result = await CarrierController.GetAssetsDataFromDb(UnitOfWork, Mapper, TransferSettings, organisationId);

        return View("/Views/Carrier/Assets.cshtml", result);
    }


}