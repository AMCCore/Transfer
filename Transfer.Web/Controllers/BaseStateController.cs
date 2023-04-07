using AutoMapper;
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

    public virtual async Task SetNextStates(SetNextStatesDto input, CancellationToken token = default)
    {
        var ns = await GetNextStatesFromDB(input).ToListAsync(token);

        input.Model.NextStates = ns.Select(x => new NextStateDto
        {
            NextStateId = x.ToStateId,
            ButtonName = x.ActionName,
            ConfirmText = (!string.IsNullOrWhiteSpace(x.ConfirmText) ? x.ConfirmText : null)
        }).ToList();
    }

    protected virtual IQueryable<DbStateMachineAction> GetNextStatesFromDB(SetNextStatesDto input)
    {
        var q = UnitOfWork.GetSet<DbStateMachineAction>()
            .Where(x => !x.IsSystemAction && x.StateMachine == input.StateMachine &&
            x.FromStates.Any(y => y.StateMachine == input.StateMachine && y.FromStateId == input.Model.State)).AsQueryable();

        return q;
    }
}
