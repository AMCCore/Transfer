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
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Transfer.Bl.Dto.Carrier;
using Transfer.Common;
using Transfer.Dal.Entities;
using Transfer.Web.Models;
using Transfer.Web.Models.Carrier;

namespace Transfer.Web.Controllers
{
    [Authorize]
    public class СarrierController : BaseController
    {
        public СarrierController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork,
            ILogger<СarrierController> logger, IMapper mapper)
            : base(transferSettings, unitOfWork, logger, mapper)
        {
        }

        private async Task<СarrierSearchFilter> GetDataFromDb(СarrierSearchFilter filter = null)
        {
            filter ??= new СarrierSearchFilter(new List<СarrierSearchResultItem>(), TransferSettings.TablePageSize);
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

            filter.Results = new CommonPagedList<СarrierSearchResultItem>(
                entity.Select(ss => Mapper.Map<СarrierSearchResultItem>(ss)).ToList(),
                filter.PageNumber, filter.PageSize, totalCount);

            return filter;
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var result = await GetDataFromDb();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm] СarrierSearchFilter filter)
        {
            var result = await GetDataFromDb(filter);

            return View(result);
        }
    }
}