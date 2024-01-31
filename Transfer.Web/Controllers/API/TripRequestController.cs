using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transfer.Common.Security;
using Transfer.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Collections.Generic;
using Transfer.Bl.Dto;
using Transfer.Bl.Dto.TripRequest;

namespace Transfer.Web.Controllers.API;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TripRequestController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityService _securityService;

    public TripRequestController(IUnitOfWork unitOfWork, ISecurityService securityService, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _securityService = securityService;
        _tokenService = tokenService;
    }

    [ProducesResponseType(typeof(EntityParametrDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    [Route(nameof(GetTripRequestParams))]
    public async Task<IActionResult> GetTripRequestParams(CancellationToken token = default)
    {
        throw new System.NotImplementedException();
    }

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create([FromBody] ITripRequest model, CancellationToken token = default)
    {
        throw new System.NotImplementedException();
    }

}
