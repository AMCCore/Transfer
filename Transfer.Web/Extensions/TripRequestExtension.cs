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
        var qTrips = unitOfWork.GetSet<DbTripRequest>().Where(x => !x.IsDeleted);
        qTrips = qTrips.Where(x =>
        (x.State == TripRequestStateEnum.Active && x.TripDate < DateTime.Now) //не выбран перевозчик а дата поездки наступила - надо сделать просроченным
        || x.State == TripRequestStateEnum.Canceled //отменён - надо списать в архив
        || x.State == TripRequestStateEnum.Overdue //просрочен - надо списать в архив
        || x.State == TripRequestStateEnum.Completed //завершен - надо списать в архив
        );

        foreach (var t in await qTrips.ToListAsync())
        {
            if(t.State == TripRequestStateEnum.Active)
            {
                t.State = TripRequestStateEnum.Archived;
                await unitOfWork.SaveChangesAsync();

                await unitOfWork.AddEntityAsync(new DbHistoryLog
                {
                    AccountId = Guid.Empty,
                    EntityId = t.Id,
                    Description = $"{TripRequestStateEnum.Active.GetEnumDescription()} -> {TripRequestStateEnum.Overdue.GetEnumDescription()}",
                    ActionName = "Ситемный перевод статусов",
                }, CancellationToken.None);

                await unitOfWork.AddEntityAsync(new DbHistoryLog
                {
                    AccountId = Guid.Empty,
                    EntityId = t.Id,
                    Description = $"{TripRequestStateEnum.Overdue.GetEnumDescription()} -> {TripRequestStateEnum.Archived.GetEnumDescription()}",
                    ActionName = "Ситемный перевод статусов",
                }, CancellationToken.None);
            }
            else if(t.State == TripRequestStateEnum.Canceled || t.State == TripRequestStateEnum.Overdue || t.State == TripRequestStateEnum.Completed)
            {
                t.State = TripRequestStateEnum.Archived;
                await unitOfWork.SaveChangesAsync();

                await unitOfWork.AddEntityAsync(new DbHistoryLog
                {
                    AccountId = Guid.Empty,
                    EntityId = t.Id,
                    Description = $"{t.State.GetEnumDescription()} -> {TripRequestStateEnum.Archived.GetEnumDescription()}",
                    ActionName = "Ситемный перевод статусов",
                }, CancellationToken.None);
            }
        }

        unitOfWork.Commit();
    }
}
