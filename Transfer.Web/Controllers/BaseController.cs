using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Transfer.Common;
using Transfer.Dal.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Transfer.Web.Models;
using Transfer.Common.Extensions;
using System.Threading.Tasks;

namespace Transfer.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public abstract class BaseController : Controller
{
    protected readonly ILogger Logger;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly TransferSettings TransferSettings;
    protected readonly IMapper Mapper;

    protected BaseController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
    {
        TransferSettings = transferSettings.Value;
        UnitOfWork = unitOfWork;
        Logger = logger;
        Mapper = mapper;
    }

    public bool HasRight<TEnum>(TEnum t) where TEnum : Enum
    {
        return UnitOfWork.GetSet<DbAccountRight>().Any(x => x.AccountId == Security.CurrentAccountId && (x.RightId == t.GetEnumGuid() || x.RightId == Common.Enums.AccessRights.AdminAccessRights.IsAdmin.GetEnumGuid()));
    }

    public async Task<bool> HasRightAsync<TEnum>(TEnum t) where TEnum : Enum
    {
        return await UnitOfWork.GetSet<DbAccountRight>().AnyAsync(x => x.AccountId == Security.CurrentAccountId && (x.RightId == t.GetEnumGuid() || x.RightId == Common.Enums.AccessRights.AdminAccessRights.IsAdmin.GetEnumGuid()));
    }




}

