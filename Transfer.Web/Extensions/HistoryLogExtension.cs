using System;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Web.Extensions;

public static class HistoryLogExtension
{
    /// <summary>
    /// Add object log information
    /// </summary>
    public async static Task AddToHistoryLog(this IUnitOfWork unitOfWork, IEntityBase Entity, string ActionName, string Description = null, CancellationToken token = default)
    {
        await unitOfWork.AddToHistoryLog(Entity.Id, Moduls.Security.CurrentAccountId, ActionName, Description, token);
    }

    /// <summary>
    /// Add object log information
    /// </summary>
    public async static Task AddToHistoryLog(this IUnitOfWork unitOfWork, IEntityBase Entity, Guid AccountId, string ActionName, string Description = null, CancellationToken token = default)
    {
        await unitOfWork.AddToHistoryLog(Entity.Id, Moduls.Security.CurrentAccountId, ActionName, Description, token);
    }

    /// <summary>
    /// Add object log information
    /// </summary>
    public async static Task AddToHistoryLog(this IUnitOfWork unitOfWork, Guid EntityId, string ActionName, string Description = null, CancellationToken token = default)
    {
        await unitOfWork.AddToHistoryLog(EntityId, Moduls.Security.CurrentAccountId, ActionName, Description, token);
    }

    /// <summary>
    /// Add object log information
    /// </summary>
    public async static Task AddToHistoryLog(this IUnitOfWork unitOfWork, Guid EntityId, Guid AccountId, string ActionName, string Description = null, CancellationToken token = default)
    {
        if(EntityId.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(EntityId));
        }

        await unitOfWork.AddEntityAsync(new DbHistoryLog { 
            AccountId = AccountId,
            EntityId = EntityId,
            Description = Description,
            ActionName = ActionName,
        }, token: token);
    }
}
