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
using Transfer.Bl.Dto.Organisation;
using Transfer.Common;
using Transfer.Common.Extensions;
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

        return View("CarrierAccountSave", new OrganisationAccountDto { OrganisationId = carrierId, OrganisationName = entity.Name });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> CarrierAccountSave([FromForm] OrganisationAccountDto accountModel)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == accountModel.OrganisationId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("CarrierAccountSave", accountModel);
        }

        if (accountModel.Id.IsNullOrEmpty())
        {
            if(await UnitOfWork.GetSet<DbAccount>().AnyAsync(ss => ss.Email == accountModel.Email, CancellationToken.None))
            {
                ViewBag.ErrorMsg = "Пользователь с таким Email уже имеется в системе";
                return View("CarrierAccountSave", accountModel);
            }

            var account = new DbAccount {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Phone = accountModel.Phone,
                Email = accountModel.Email,
                PersonData = new DbPersonData { 
                    FirstName = accountModel.FirstName,
                    LastName = accountModel.LastName,
                    MiddleName = accountModel.MiddleName,
                    DocumentSeries = string.Empty,
                    DocumentNumber = string.Empty,
                    DocumentSubDivisionCode = string.Empty,
                    DocumentIssurer = string.Empty,
                    DocumentDateOfIssue = DateTime.MinValue,
                    RegistrationAddress = string.Empty,
                },
                Password = BCrypt.Net.BCrypt.HashString(Guid.NewGuid().ToString())

        };

            await UnitOfWork.AddEntityAsync(account, CancellationToken.None);

            accountModel.Id = account.Id;
            await UnitOfWork.AddEntityAsync(new DbOrganisationAccount { 
                AccountId = account.Id,
                OrganisationId = accountModel.OrganisationId,
                AccountType = Common.Enums.OrganisationAccountType.Operator
            }, CancellationToken.None);
        }
        else
        {
            var account = await UnitOfWork.GetSet<DbAccount>().Include(x => x.PersonData).FirstOrDefaultAsync(ss => ss.Id == accountModel.Id, CancellationToken.None);

            if (account.LastUpdateTick != accountModel.LastUpdateTick)
                throw new InvalidOperationException();

            account.Phone = accountModel.Phone;
            account.PersonData.FirstName = accountModel.FirstName;
            account.PersonData.LastName = accountModel.LastName;
            account.PersonData.MiddleName = accountModel.MiddleName;

            await UnitOfWork.SaveChangesAsync();
        }

        return RedirectToAction(nameof(CarrierController.CarrierItem), "Carrier", new { carrierId = accountModel.OrganisationId });
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Account/{accountId}")]
    public async Task<IActionResult> CarrierAccountItem([Required] Guid carrierId, [Required] Guid accountId)
    {
        var org = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (org == null)
            return NotFound();

        var entity = await UnitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(ss => ss.Organisations.Any(x => x.OrganisationId == org.Id) && ss.Id == accountId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        var res = Mapper.Map<OrganisationAccountDto>(entity);
        res.OrganisationId = org.Id;
        res.OrganisationName = org.Name;

        return View("CarrierAccountSave", res);
    }
}
