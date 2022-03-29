using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Transfer.Bl.Dto;
using Transfer.Common;

namespace Transfer.Web.Controllers;

public abstract class BaseStateController : BaseController
{
    public BaseStateController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<BaseStateController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    public void SetNextStates(StateMachineDto model)
    {
        model.NextStates = GetPossibleStatets(model.State);
    }

    public virtual IDictionary<Guid, string> GetPossibleStatets(Guid currentState)
    {
        return new Dictionary<Guid, string>();
    }

    public virtual bool CanSetState(StateMachineDto model)
    {
        return model.StateChange.HasValue && GetPossibleStatets(model.State).ContainsKey(model.StateChange.Value);
    }
}
