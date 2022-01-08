using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Transfer.Common;
using System;

namespace Transfer.Web.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
            : base(transferSettings, unitOfWork, logger, mapper)
        {

        }

        public static bool Authenticate(string login, string password, bool persistent)
        {
            //var _authenticationManager = GetOwinContext().Authentication;
            throw new NotImplementedException();
        }
    }
}
