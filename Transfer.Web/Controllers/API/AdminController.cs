using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Transfer.Common.Settings;
using Transfer.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Transfer.Bl.Dto;
using Transfer.Common.Enums.AccessRights;
using System;
using Transfer.Dal.Entities;
using Transfer.Dal.Migrations;
using Transfer.Common.Extensions;
using System.Threading;
using Transfer.Dal;
using System.Linq;
using Transfer.Common.Enums.States;
using Microsoft.EntityFrameworkCore;
using Transfer.Web.Extensions;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdminController(IWebHostEnvironment appEnvironment, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _appEnvironment = appEnvironment;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(nameof(CheckHealth))]
    public async Task<IActionResult> CheckHealth(CancellationToken token = default)
    {
        try
        {
            var d1 = DateTime.Now;
            var trr1 = await _unitOfWork.GetSet<DbTripRequest>().Where(x => x.State == TripRequestStateEnum.Active.GetEnumGuid() && x.TripDate < d1).ToListAsync(token);
            foreach (var tr in trr1)
            {
                tr.State = TripRequestStateEnum.Overdue.GetEnumGuid();
                await _unitOfWork.SaveChangesAsync(token);
                await _unitOfWork.AddToHistoryLog(tr, "Статус запроса на перевозку изменён системой (дата поездки наступила а перевозчик не выбран)", $"Новый статус: {TripRequestStateEnum.Overdue.GetEnumDescription()}", token);
            }

            var d2 = d1.AddDays(-15);
            var sts = new[] { TripRequestStateEnum.Overdue.GetEnumGuid(), TripRequestStateEnum.Canceled.GetEnumGuid(), TripRequestStateEnum.Completed.GetEnumGuid(), TripRequestStateEnum.CompletedNoConfirm.GetEnumGuid() };
            var trr2 = await _unitOfWork.GetSet<DbTripRequest>().Where(x => sts.Contains(x.State) && x.LastUpdateTick <= d2.Ticks).ToListAsync(token);

            foreach (var tr in trr1)
            {
                tr.State = TripRequestStateEnum.Archived.GetEnumGuid();
                await _unitOfWork.SaveChangesAsync(token);
                await _unitOfWork.AddToHistoryLog(tr, "Статус запроса на перевозку изменён системой (Перенесено в архив)", $"Новый статус: {TripRequestStateEnum.Archived.GetEnumDescription()}", token);
            }
        }
        catch
        {

        }

        return Ok();
    }

    [HttpPost]
    [Route(nameof(AddAdminUser))]
    public async Task<IActionResult> AddAdminUser([FromBody] AccountWithPassDto account)
    {
        if (!Moduls.Security.Current.HasRightForSomeOrganisation(AdminAccessRights.IsAdmin))
        {
            return Unauthorized();
        }

        await _unitOfWork.BeginTransactionAsync();

        var pd = new DbPersonData
        {
            Id = Guid.NewGuid(),
            FirstName = account.FirstName,
            LastName = account.LastName,
            MiddleName = account.MiddleName ?? string.Empty,
            BirthDate = account.BirthDate ?? new DateTime(1900, 3, 6),
            IsMale = true,
            DocumentSeries = string.Empty,
            DocumentNumber = string.Empty,
            DocumentSubDivisionCode = string.Empty,
            DocumentIssurer = string.Empty,
            DocumentDateOfIssue = DateTime.MinValue,
            RegistrationAddress = string.Empty
        };

        await _unitOfWork.AddEntityAsync(pd);

        var acc = new DbAccount
        {
            Id = Guid.NewGuid(),
            PersonDataId = pd.Id,
            LastUpdateTick = DateTime.Now.Ticks,
            DateCreated = DateTime.Now,
            Password = BCrypt.Net.BCrypt.HashString(account.Password),
            Email = account.Email,
        };

        await _unitOfWork.AddEntityAsync(acc);

        await _unitOfWork.AddEntityAsync(new DbAccountRight
        {
            Id = Guid.NewGuid(),
            AccountId = acc.Id,
            RightId = AdminAccessRights.IsAdmin.GetEnumGuid(),
            OrganisationId = null
        });

        await _unitOfWork.CommitAsync();

        return Ok(acc.Id);
    }
}
