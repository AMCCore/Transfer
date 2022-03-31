using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Transfer.Common;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Transfer.Web.Controllers;
[Authorize]
public class TransportController : BaseController
{
    public TransportController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<TransportController> logger, IMapper mapper)
        : base(transferSettings, unitOfWork, logger, mapper)

    {
    }

    [Route("Transports")]
    public IActionResult Index()
    {
        throw new NotImplementedException();
    }

    public IActionResult Search()
    {
        throw new NotImplementedException();
    }

}
