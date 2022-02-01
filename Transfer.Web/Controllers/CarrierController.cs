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
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            query = query.Where(x => x.Address.ToLower().Contains(filter.City.ToLower()));
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
    public async Task<IActionResult> SearchCarrier([FromForm] CarrierSearchFilter filter)
    {
        var result = await GetDataFromDb(filter);

        return PartialView("SearchResults", result);
    }

    [HttpGet]
    [Route("Carrier/New")]
    public IActionResult NewCarrier()
    {
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

            return View("CarrierEdit", res);
        }
        return NotFound();
    }

    [AutoValidateAntiforgeryToken]
    [HttpPost]
    [Route("Carrier/Save")]
    public async Task<IActionResult> Save([FromForm] CarrierDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("Carrier", model);
        }

        if (!model.Agreement)
        {
            ViewBag.ErrorMsg = "Ошибка согласия перс данных";
            return View("Carrier", model);
        }
        if (string.IsNullOrWhiteSpace(model.City))
        {
            model.City = "Неизвестный город";
        }

        if (model.Id.IsNullOrEmpty())
        {
            var org = Mapper.Map<DbOrganisation>(model);
            org.Id = Guid.NewGuid();
            org.DirectorFio = string.Empty;
            model.Id = org.Id;
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

        filter.Results = new CommonPagedList<OrganisationAssetDto>(entitys, filter.PageNumber, filter.PageSize, totalCount);

        return filter;
    }

    public async Task<IActionResult> OrganisationAssets(Guid organisationId)
    {
        var result = await GetAssetsDataFromDb(UnitOfWork, Mapper, TransferSettings, organisationId);

        return PartialView("Assets", result);
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