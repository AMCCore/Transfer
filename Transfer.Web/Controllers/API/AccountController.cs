using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Common;
using Transfer.Common.Utils;

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
    private async Task<IActionResult> Register([FromBody] AccountWithPassAndPhoneDto model, CancellationToken token = default)
    {
        if (!EMailValidator.IsValidEmail(model.Email))
        {
            return BadRequest("");
        }
    }
}
