using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Transfer.Common;

namespace Transfer.Web.Controllers;

[Authorize]
public class TripController : BaseController
{
    public TripController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TripController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    [Route("Trips")]
    public IActionResult Index()
    {
        throw new NotImplementedException();
    }
}