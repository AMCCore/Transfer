using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Settings;
using Transfer.Dal.Entities;

namespace Transfer.Web.Controllers;

[Authorize]
public class AccountController : BaseController
{
    public AccountController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<AccountController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    [Route("Account")]
    public IActionResult Index()
    {
        throw new NotImplementedException();
    }

    public IActionResult Reviews()
    {
        throw new NotImplementedException();
    }


    [HttpGet]
    [Route("Carrier/{carrierId}/Account/New")]
    public async Task<IActionResult> CarrierNewAccount([Required] Guid carrierId)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        throw new NotImplementedException();
        //return View("Save", new BusDto { OrganisationId = carrierId, OrganisationName = entity.Name });
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Account/{accountId}")]
    public async Task<IActionResult> CarrierAccountItem([Required] Guid carrierId, [Required] Guid accountId)
    {
        var entity = await UnitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(ss => ss.Organisations.Any(x => x.OrganisationId == carrierId) && ss.Id == accountId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        throw new NotImplementedException();

        //var res = Mapper.Map<BusDto>(entity);

        //return View("Save", res);
    }
}
