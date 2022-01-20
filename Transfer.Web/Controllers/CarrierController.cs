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
        return View("Carrier", new CarrierDto());
    }

    [HttpGet]
    [Route("Carrier/{carrierId}")]
    public async Task<IActionResult> CarrierItem(Guid carrierId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if(entity != null)
        {
            var res = Mapper.Map<CarrierDto>(entity);
            Mapper.Map(entity.BankDetails.FirstOrDefault(x => !x.IsDeleted), res);

            return View("Carrier", res);
        }
        return NotFound();
    }

    [AutoValidateAntiforgeryToken]
    [HttpPost]
    [Route("Carrier/Save")]
    public async Task<IActionResult> Save([FromForm] CarrierDto model)
    {
        
        return View("Carrier", model);
    }

}