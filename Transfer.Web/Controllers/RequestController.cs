using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Transfer.Common;

namespace Transfer.Web.Controllers
{
    [Authorize]
    public class RequestController : BaseController
    {
        public RequestController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<RequestController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
        {
        }
        
        [Route("Requests")]
        [HttpGet]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}