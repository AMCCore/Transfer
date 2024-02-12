using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Common;
using Transfer.Common.Utils;
using Transfer.Dal.Entities;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AccountController(IWebHostEnvironment appEnvironment, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _appEnvironment = appEnvironment;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [Route(nameof(Register))]
    public async Task<IActionResult> Register([FromBody] AccountWithPassAndPhoneDto model, CancellationToken token = default)
    {
        if (
            string.IsNullOrEmpty(model.FirstName) ||
            string.IsNullOrEmpty(model.LastName) ||
            string.IsNullOrEmpty(model.Phone) ||
            string.IsNullOrEmpty(model.Email) ||
            string.IsNullOrEmpty(model.Password)
            )
        {
            return BadRequest("Model not valid");
        }

        if (!EMailValidator.IsValidEmail(model.Email))
        {
            return BadRequest("Email not valid");
        }

        if (await _unitOfWork.GetSet<DbAccount>().AnyAsync(ss => ss.Email == model.Email, token))
        {
            return BadRequest("Email exists");
        }

        if (await _unitOfWork.GetSet<DbAccount>().AnyAsync(ss => ss.Phone == model.Phone, token))
        {
            return BadRequest("Phone exists");
        }

        var account = new DbAccount
        {
            Id = Guid.NewGuid(),
            IsDeleted = false,
            Phone = model.Phone,
            Email = model.Email,
            PersonData = new DbPersonData
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName ?? string.Empty,
                DocumentSeries = string.Empty,
                DocumentNumber = string.Empty,
                DocumentSubDivisionCode = string.Empty,
                DocumentIssurer = string.Empty,
                DocumentDateOfIssue = DateTime.MinValue,
                RegistrationAddress = string.Empty,
            },
            Password = BCrypt.Net.BCrypt.HashString(model.Password)
        };

        await _unitOfWork.AddEntityAsync(account, token: token);

        return Ok(new { UserId = account.Id.ToString(), model.Phone, model.Email });
    }
}
