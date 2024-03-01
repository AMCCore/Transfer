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
using Org.BouncyCastle.Crypto.Engines;

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

        if (!string.IsNullOrWhiteSpace(filter.InnName))
        {
            query = query.Where(x => x.INN.ToLower().Contains(filter.InnName.ToLower()) || x.Name.ToLower().Contains(filter.InnName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.Phone))
        {
            query = query.Where(x => x.Phone.ToLower().Contains(filter.Phone.ToLower()) || x.Accounts.Any(y=>y.Account.Phone.ToLower().Contains(filter.Phone.ToLower())));
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            //query = query.Where(x => x.Address.ToLower().Contains(filter.City.ToLower()) || x.FactAddress.ToLower().Contains(filter.City.ToLower()) || x.City.ToLower().Contains(filter.City.ToLower()));
            query = query.Where(x => !x.WorkingArea.Any() || x.WorkingArea.Any(x => x.Region.Name.ToLower().Contains(filter.City.ToLower())));
        }

        if (filter.ActiveOnly)
        {
            query = query.Where(x => !x.IsInactive);
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
    public async Task<IActionResult> Save([FromForm] CarrierDto model, CancellationToken token = default)
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

        if (model.WorkingAreas?.All(string.IsNullOrWhiteSpace) ?? true)
        {
            ViewBag.ErrorMsg = "Необходимо указать регион(ы) работы";
            ViewBag.Regions = await GetRegionsAsync();
            return View("CarrierEdit", model);
        }

        if (!string.IsNullOrWhiteSpace(model.INN) && await UnitOfWork.GetSet<DbOrganisation>().AnyAsync(x => x.Id != model.Id && !x.IsDeleted && x.INN.ToLower() == model.INN.ToLower(), token))
        {
            ViewBag.ErrorMsg = "Организация с таким ИНН уже существует";
            ViewBag.Regions = await GetRegionsAsync();
            return View("CarrierEdit", model);
        }

        await UnitOfWork.BeginTransactionAsync(token);

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

            await UnitOfWork.AddEntityAsync(org, token: token);
            var br = Mapper.Map<DbBankDetails>(model);
            br.Id = Guid.NewGuid();
            br.OrganisationId = org.Id;
            await UnitOfWork.AddEntityAsync(br, token: token);
        }
        else
        {
            var org = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == model.Id, token);
            if (org == null)
            {
                NotFound();
            }
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

            await UnitOfWork.SaveChangesAsync(token);

            var brd = await UnitOfWork.GetSet<DbBankDetails>().Where(x => !x.IsDeleted && x.OrganisationId == org.Id).ToListAsync(token);
            foreach (var b in brd)
            {
                b.IsDeleted = true;
            }
            await UnitOfWork.SaveChangesAsync(token);

            var br = Mapper.Map<DbBankDetails>(model);
            br.Id = Guid.NewGuid();
            br.OrganisationId = org.Id;
            await UnitOfWork.AddEntityAsync(br, token: token);
        }

        //лицензия
        await SetCarrierFile(model.Id, model.LicenceFileId, Common.Enums.OrganisationFileTypeEnum.License, token);
        //логотип
        await SetCarrierFile(model.Id, model.LogoFileId, Common.Enums.OrganisationFileTypeEnum.Logo, token);
        //регионы работы
        await SetCarrierWorkingArea(model.Id, model.WorkingAreas, token);

        await UnitOfWork.CommitAsync(token);

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

    private async Task SetCarrierFile(Guid carrierId, Guid? fileId, Common.Enums.OrganisationFileTypeEnum fileType, CancellationToken token = default)
    {
        var files = await UnitOfWork.GetSet<DbOrganisationFile>().Where(x => x.OrganisationId == carrierId && !x.IsDeleted && x.FileType == fileType).ToListAsync(token);
        var tsks = new List<Task>();
        if (files.All(x => x.FileId != fileId))
        {
            tsks.Add(UnitOfWork.DeleteListAsync(files, token));

            if (!fileId.IsNullOrEmpty())
            {
                tsks.Add(UnitOfWork.AddEntityAsync(new DbOrganisationFile
                {
                    OrganisationId = carrierId,
                    FileId = fileId.Value,
                    IsDeleted = false,
                    FileType = fileType,
                    UploaderId = Security.CurrentAccountId
                }, token: token));
;            }
        }
        await Task.WhenAll(tsks);

    }

    private async Task<IDictionary<Guid, string>> GetRegionsAsync(params Guid[] exisitingRegs)
    {
        return await UnitOfWork.GetSet<DbRegion>().ToDictionaryAsync(x => x.Id, y => y.Name, CancellationToken.None);
    }

    private async Task SetCarrierWorkingArea(Guid carrierId, IEnumerable<string> regions, CancellationToken token = default)
    {
        var eRegions = await UnitOfWork.GetSet<DbOrganisationWorkingArea>().Where(x => x.OrganisationId == carrierId).ToListAsync(token);
        await UnitOfWork.DeleteListAsync(eRegions, token);

        foreach (var region in regions)
        {
            if (Guid.TryParse(region, out var regId) && UnitOfWork.GetSet<DbRegion>().Any(x => x.Id == regId))
            {
                await UnitOfWork.AddEntityAsync(new DbOrganisationWorkingArea { 
                    RegionId = regId,
                    OrganisationId = carrierId
                }, token: token);
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