using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Bl.Dto.Organisation;
using Transfer.Common;
using Transfer.Common.Extensions;
using Transfer.Common.Settings;
using Transfer.Common.Utils;
using Transfer.Dal.Entities;
using Transfer.Web.Extensions;
using Transfer.Web.Moduls;
using X.PagedList;

namespace Transfer.Web.Controllers;

[Authorize]
public class AccountController : BaseController
{
    private IMailModule MailModule;

    public AccountController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<AccountController> logger, IMapper mapper, IMailModule mailModule) : base(transferSettings, unitOfWork, logger, mapper)
    {
        MailModule = mailModule;
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
    public async Task<IActionResult> CarrierAccountSave([FromForm] OrganisationAccountDto accountModel, CancellationToken token = default)
    {
        var entity = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == accountModel.OrganisationId, token);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Одно или несколько полей не заполнены";
            return View("CarrierAccountSave", accountModel);
        }

        if (accountModel.Id.IsNullOrEmpty())
        {
            if(await UnitOfWork.GetSet<DbAccount>().AnyAsync(ss => ss.Email == accountModel.Email, token))
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
                    MiddleName = accountModel.MiddleName ?? string.Empty,
                    DocumentSeries = string.Empty,
                    DocumentNumber = string.Empty,
                    DocumentSubDivisionCode = string.Empty,
                    DocumentIssurer = string.Empty,
                    DocumentDateOfIssue = DateTime.MinValue,
                    RegistrationAddress = string.Empty,
                },
                Password = BCrypt.Net.BCrypt.HashString(Guid.NewGuid().ToString())

        };

            await UnitOfWork.AddEntityAsync(account, token: token);

            accountModel.Id = account.Id;
            await UnitOfWork.AddEntityAsync(new DbOrganisationAccount { 
                AccountId = account.Id,
                OrganisationId = accountModel.OrganisationId,
                AccountType = Common.Enums.OrganisationAccountTypeEnum.Operator
            }, token: token);
        }
        else
        {
            var account = await UnitOfWork.GetSet<DbAccount>().Include(x => x.PersonData).FirstOrDefaultAsync(ss => ss.Id == accountModel.Id, token);

            if (account.LastUpdateTick != accountModel.LastUpdateTick)
                throw new InvalidOperationException();

            if (await UnitOfWork.GetSet<DbAccount>().AnyAsync(x => x.Email.ToLower() == accountModel.Email.ToLower() && x.Id != accountModel.Id, token))
                throw new InvalidOperationException();

            account.Phone = accountModel.Phone;
            account.PersonData.FirstName = accountModel.FirstName;
            account.PersonData.LastName = accountModel.LastName;
            account.PersonData.MiddleName = accountModel.MiddleName ?? string.Empty;
            account.Email = accountModel.Email;

            await UnitOfWork.SaveChangesAsync(token);
        }

        return RedirectToAction(nameof(CarrierAccountItem), new { carrierId = accountModel.OrganisationId, accountId = accountModel.Id });
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

    [HttpGet]
    [Route("Carrier/{carrierId}/Account/{accountId}/DropPassword")]
    public async Task<IActionResult> CarrierAccountDropPassword([Required] Guid carrierId, [Required] Guid accountId, CancellationToken token = default)
    {
        var org = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, token);
        if (org == null)
            return NotFound();

        var entity = await UnitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(ss => ss.Organisations.Any(x => x.OrganisationId == org.Id) && ss.Id == accountId, token);
        if (entity == null)
            return NotFound();

        var newPass = Password.Generate();

        entity.Password = BCrypt.Net.BCrypt.HashString(newPass);
        await UnitOfWork.SaveChangesAsync(token);

        await MailModule.SendEmailPlainTextAsync($"Новый пароль для входа: {newPass}", "Новый пароль для входа", entity.Email, true);

        var res = Mapper.Map<OrganisationAccountDto>(entity);
        res.OrganisationId = org.Id;
        res.OrganisationName = org.Name;
        ViewBag.ErrorMsg = "Новый пароль отправлен пользователю.";
        return View("CarrierAccountSave", res);
    }

    [HttpGet]
    [Route("Carrier/{carrierId}/Account/{accountId}/Delete")]
    public async Task<IActionResult> CarrierAccountItemDelete([Required] Guid carrierId, [Required] Guid accountId)
    {
        var org = await UnitOfWork.GetSet<DbOrganisation>().FirstOrDefaultAsync(ss => ss.Id == carrierId, CancellationToken.None);
        if (org == null)
            return NotFound();

        var entity = await UnitOfWork.GetSet<DbAccount>().FirstOrDefaultAsync(ss => ss.Organisations.Any(x => x.OrganisationId == org.Id) && ss.Id == accountId, CancellationToken.None);
        if (entity == null)
            return NotFound();

        await UnitOfWork.BeginTransactionAsync(CancellationToken.None);

        entity.IsDeleted = true;
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        var els = await entity.ExternalLogins.ToListAsync(CancellationToken.None);
        await UnitOfWork.DeleteListAsync(els, CancellationToken.None);

        await UnitOfWork.CommitAsync(CancellationToken.None);

        return RedirectToAction(nameof(CarrierController.CarrierItem), typeof(CarrierController).ControllerName(), new { carrierId = org.Id });
    }
}
