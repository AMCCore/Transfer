using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Transfer.Common;

namespace Transfer.Web.Controllers
{
    [AllowAnonymous]
    public class InfoController : BaseController
    {
        public InfoController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<InfoController> logger, IMapper mapper)
            : base(transferSettings, unitOfWork, logger, mapper)
        {
        }

        public IActionResult About()
        {
            throw new NotImplementedException();
        }

        public IActionResult Normatives()
        {
            throw new NotImplementedException();
        }

        [Route("Contacts")]
        public IActionResult Contacts()
        {
            throw new NotImplementedException();
        }

        [Route("Specials")]
        public IActionResult Specials()
        {
            throw new NotImplementedException();
        }

        public IActionResult Documents()
        {
            throw new NotImplementedException();
        }

        public IActionResult Vacancy()
        {
            throw new NotImplementedException();
        }
    }
}
