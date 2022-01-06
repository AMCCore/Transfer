using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Transfer.Common;

namespace Transfer.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger Logger;
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly TransferSettings TransferSettings;

        protected BaseController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger logger)
        {
            TransferSettings = transferSettings.Value;
            UnitOfWork = unitOfWork;
            Logger = logger;
        }

    }
}
