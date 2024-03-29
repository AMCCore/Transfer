﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Bl.Dto;
using Transfer.Common;
using Transfer.Common.Enums.States;
using Transfer.Common.Settings;
using Transfer.Dal.Entities;

namespace Transfer.Web.Controllers;

public abstract class BaseStateController : BaseController
{
    public BaseStateController(IOptions<TransferSettings> transferSettings, IUnitOfWork unitOfWork, ILogger<BaseStateController> logger, IMapper mapper) : base(transferSettings, unitOfWork, logger, mapper)
    {
    }

    public virtual async Task SetNextStates(StateMachineDto model, StateMachineEnum stateMachine, Guid? organisationId = null, CancellationToken token = default)
    {
        var ns = await GetNextStatesFromDB(model, stateMachine, organisationId).ToListAsync(token);

        model.NextStates = ns.Select(x => new NextStateDto
        {
            NextStateId = x.ToStateId,
            ButtonName = x.ActionName,
            ConfirmText = (!string.IsNullOrWhiteSpace(x.ConfirmText) ? x.ConfirmText : null)
        }).ToList();
    }

    protected virtual IQueryable<DbStateMachineAction> GetNextStatesFromDB(StateMachineDto model, StateMachineEnum stateMachine, Guid? organisationId = null)
    {
        var q = UnitOfWork.GetSet<DbStateMachineAction>()
            .Where(x => !x.IsSystemAction && x.StateMachine == stateMachine &&
            x.FromStates.Any(y => y.StateMachine == stateMachine && y.FromStateId == model.State)).AsQueryable();

        return q;
    }
}
