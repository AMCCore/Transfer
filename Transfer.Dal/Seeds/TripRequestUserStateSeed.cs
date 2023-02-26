using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Dal.Seeds;

internal static class TripRequestUserStateSeed
{
    internal static void SetTripRequestUserStates(this IUnitOfWork uw)
    {
        var tripRequestUserStates = new List<DbStateMachineState>
        {
            new DbStateMachineState {
                Id = TripRequestStateEnum.New.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.New.GetEnumDescription(),
                Description = "Новый заказ"
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.Active.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.Active.GetEnumDescription(),
                Description = "Cбор предложений/откликов перевозчиков"
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.CarrierSelected.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.CarrierSelected.GetEnumDescription(),
                Description = "Заключение договора"
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.Done.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.Done.GetEnumDescription(),
                Description = "Ожидание подтверждения"
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.Completed.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.Completed.GetEnumDescription(),
                Description = null
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.Canceled.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.Canceled.GetEnumDescription(),
                Description = null
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.Overdue.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.Overdue.GetEnumDescription(),
                Description = null
            },
            //new DbStateMachineState {
            //    Id = Guid.Parse("A005D7AC-9B2D-4234-A78A-0FDCF3659C5F"),
            //    StateMachine = StateMachineEnum.TripRequest,
            //    IsDeleted = false,
            //    Name = "Изменение перевозчика",
            //    Description = null
            //},
            new DbStateMachineState {
                Id = TripRequestStateEnum.CompletedNoConfirm.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.CompletedNoConfirm.GetEnumDescription(),
                Description = null
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.Archived.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.Archived.GetEnumDescription(),
                Description = null
            },
        };
        uw.AddIfNotExists(tripRequestUserStates);
    }
}
