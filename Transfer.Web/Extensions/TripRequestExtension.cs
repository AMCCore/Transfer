using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Web.Extensions;

public static class TripRequestExtension
{
    public async static Task TripRequestStateRegulate(this IUnitOfWork unitOfWork)
    {
        unitOfWork.BeginTransaction();
        var qTrips = unitOfWork.GetSet<DbTripRequest>().AsQueryable();
        qTrips = qTrips.Where(x =>
        (x.State == TripRequestStateEnum.Active.GetEnumGuid() && x.TripDate < DateTime.Now) //не выбран перевозчик а дата поездки наступила - надо сделать просроченным
        || x.State == TripRequestStateEnum.Canceled.GetEnumGuid() //отменён - надо списать в архив
        || x.State == TripRequestStateEnum.Overdue.GetEnumGuid() //просрочен - надо списать в архив
        || x.State == TripRequestStateEnum.Completed.GetEnumGuid() //завершен - надо списать в архив
        );

        foreach (var t in await qTrips.ToListAsync())
        {
            if(t.State == TripRequestStateEnum.Active.GetEnumGuid())
            {
                t.State = TripRequestStateEnum.Archived.GetEnumGuid();
                await unitOfWork.SaveChangesAsync();

                await unitOfWork.AddEntityAsync(new DbHistoryLog
                {
                    AccountId = Guid.Empty,
                    EntityId = t.Id,
                    Description = $"{TripRequestStateEnum.Active.GetEnumDescription()} -> {TripRequestStateEnum.Overdue.GetEnumDescription()}",
                    ActionName = "Ситемный перевод статусов",
                }, token: CancellationToken.None);

                await unitOfWork.AddEntityAsync(new DbHistoryLog
                {
                    AccountId = Guid.Empty,
                    EntityId = t.Id,
                    Description = $"{TripRequestStateEnum.Overdue.GetEnumDescription()} -> {TripRequestStateEnum.Archived.GetEnumDescription()}",
                    ActionName = "Ситемный перевод статусов",
                }, token: CancellationToken.None);
            }
            else if(t.State == TripRequestStateEnum.Canceled.GetEnumGuid() || t.State == TripRequestStateEnum.Overdue.GetEnumGuid() || t.State == TripRequestStateEnum.Completed.GetEnumGuid())
            {
                var oldState = t.StateEnum;
                t.State = TripRequestStateEnum.Archived.GetEnumGuid();
                await unitOfWork.SaveChangesAsync();

                await unitOfWork.AddEntityAsync(new DbHistoryLog
                {
                    AccountId = Guid.Empty,
                    EntityId = t.Id,
                    Description = $"{oldState.GetEnumDescription()} -> {TripRequestStateEnum.Archived.GetEnumDescription()}",
                    ActionName = "Ситемный перевод статусов",
                }, token: CancellationToken.None);
            }
        }

        unitOfWork.Commit();
    }
}
