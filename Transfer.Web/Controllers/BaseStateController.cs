using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Common;
using Transfer.Common.Settings;

namespace Transfer.Web.Controllers;

public abstract class BaseStateController : BaseController
{
    public BaseStateController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<BaseStateController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    public async Task SetNextStates(StateMachineDto model)
    {
        model.NextStates = await GetPossibleStatets(model.State);
    }

    public virtual async Task<ICollection<NextStateDto>> GetPossibleStatets(Guid currentState)
    {
        return new List<NextStateDto>();
    }
}
