using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Transfer.Common;

namespace Transfer.Web.Controllers
{
    [Authorize]
    public class СarrierController : BaseController
    {
        public СarrierController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<СarrierController> logger, IMapper mapper) 
            : base(transferSettings, unitOfWork, logger, mapper)
        {
        }

        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}
