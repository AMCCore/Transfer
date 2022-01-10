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
    public class AccountController : BaseController
    {
        public AccountController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<AccountController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
        {
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

    }
}
