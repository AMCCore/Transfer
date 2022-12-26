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
    public IActionResult CheckHealth()
    {
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
