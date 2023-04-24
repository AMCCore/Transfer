using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums.AccessRights;
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
                IsDeleted = true,
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
            new DbStateMachineState {
                Id = TripRequestStateEnum.CompletedNoConfirm.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = true,
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
            new DbStateMachineState {
                Id = TripRequestStateEnum.WaitingForEstimate.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = true,
                Name = TripRequestStateEnum.WaitingForEstimate.GetEnumDescription(),
                Description = null
            },
            new DbStateMachineState {
                Id = TripRequestStateEnum.CompletedByCreator.GetEnumGuid(),
                StateMachine = StateMachineEnum.TripRequest,
                IsDeleted = false,
                Name = TripRequestStateEnum.CompletedByCreator.GetEnumDescription(),
                Description = null
            },
        };
        uw.AddOrUpdate(tripRequestUserStates, (source, destination) => {
            destination.IsDeleted = source.IsDeleted;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.StateMachine = source.StateMachine;
        });

        #region --> отменён

        var ga = Guid.Parse("94D3666E-70C2-4F85-B5A1-A4733A0A1AAD");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "cancel",
            ActionName = "Отменить",
            Description = null,
            IsSystemAction = false,
            ToStateId = TripRequestStateEnum.Canceled.GetEnumGuid(),
            ConfirmText = "Уверены что хотите отменить?",
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("84A58E87-8B2F-42DF-8086-4367F7E9DD87"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.New.GetEnumGuid(),
                RightCode = TripRequestRights.Create.GetEnumGuid(),
                StateMachineActionId = ga,
            });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("5658448A-4C24-4A5A-8628-E90DEFFB5138"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Active.GetEnumGuid(),
                RightCode = TripRequestRights.Create.GetEnumGuid(),
                StateMachineActionId = ga,
            });

        #endregion

        #region --> просрочен

        ga = Guid.Parse("9A1CDF22-B861-4246-AF2E-DC27237AB41B");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "overdue",
            ActionName = "Просрочить",
            IsSystemAction = true,
            Description = null,
            ToStateId = TripRequestStateEnum.Overdue.GetEnumGuid()
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("B52C828C-2F1D-4C99-BFE8-2E73EC2476F5"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.New.GetEnumGuid(),
                StateMachineActionId = ga
            },
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("84A58E87-8B2F-42DF-8086-4367F7E9DD87"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Active.GetEnumGuid(),
                StateMachineActionId = ga
            }
        );

        #endregion

        #region --> перевозчик выбран
        
        ga = Guid.Parse("00AE0C75-94F1-447E-866D-23DD149C60C0");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "chooseCarrier",
            ActionName = "Выбрать перевозчика",
            IsSystemAction = true,
            Description = null,
            ToStateId = TripRequestStateEnum.CarrierSelected.GetEnumGuid()
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("56130327-0D8A-4961-ABDE-DE5801D99365"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Active.GetEnumGuid(),
                StateMachineActionId = ga,
                RightCode = TripRequestRights.CarrierChoose.GetEnumGuid()
            }
        );

        #endregion

        #region --> выполнен

        ga = Guid.Parse("568BC33D-FF44-4939-9786-D48DDFE8B966");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "done",
            ActionName = "Выполнить",
            IsSystemAction = false,
            Description = null,
            ToStateId = TripRequestStateEnum.Done.GetEnumGuid()
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("81DE65CE-F563-476A-B088-9880D6B0F47B"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.CarrierSelected.GetEnumGuid(),
                StateMachineActionId = ga,
                RightCode = TripRequestRights.Done.GetEnumGuid(),
            }
        );

        #endregion

        #region --> завершен

        ga = Guid.Parse("b4f89bef-4720-4342-99fb-e7530fa33fbc");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "complete",
            ActionName = "Завершить",
            IsSystemAction = false,
            Description = null,
            ToStateId = TripRequestStateEnum.Completed.GetEnumGuid()
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("a7ec4838-a5cd-4841-ac06-f608ac90b484"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Done.GetEnumGuid(),
                StateMachineActionId = ga,
                RightCode = TripRequestRights.Completed.GetEnumGuid(),
            }
        );

        #endregion

        #region --> Завершен без подтверждения

        ga = Guid.Parse("35580EB4-6A8A-4B97-BA44-2C5193A3DD4E");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "completedWithoutConfirm",
            ActionName = "Завершить без подтверждения",
            IsSystemAction = false,
            Description = null,
            ToStateId = TripRequestStateEnum.CompletedNoConfirm.GetEnumGuid()
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("C5C56A00-B0F2-43F1-99FA-2015C72020F1"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Done.GetEnumGuid(),
                StateMachineActionId = ga,
                RightCode = TripRequestRights.Completed.GetEnumGuid(),
            }
        );

        #endregion

        #region --> архивные

        ga = Guid.Parse("AA851CAF-4346-4126-8501-0D6EB74FE4FD");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "archive",
            ActionName = "Переместить в архив",
            IsSystemAction = true,
            Description = null,
            ToStateId = TripRequestStateEnum.Archived.GetEnumGuid()
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("32487569-C8F7-444C-BAA6-6D3821254A21"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Canceled.GetEnumGuid(),
                StateMachineActionId = ga
            },
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("552FCCC0-A248-4AA7-AF4B-0C33A5F6EA5A"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Overdue.GetEnumGuid(),
                StateMachineActionId = ga
            },
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("C773DFF2-C0D9-40E1-907A-AC09E12E687D"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.Completed.GetEnumGuid(),
                StateMachineActionId = ga
            },
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("C9FE2A90-8DE9-4D5B-A5F0-39F52D8DAB9A"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.CompletedNoConfirm.GetEnumGuid(),
                StateMachineActionId = ga
            },
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("7d6a7b93-81f7-43c6-a518-3e02545045e9"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.CompletedByCreator.GetEnumGuid(),
                StateMachineActionId = ga
            }
        );

        #endregion

        #region --> завершен оператором

        ga = Guid.Parse("343405be-6474-41a4-99d9-b2543609d36f");
        uw.AddIfNotExists(new DbStateMachineAction
        {
            Id = ga,
            IsDeleted = false,
            StateMachine = StateMachineEnum.TripRequest,
            ActionCode = "completedByCreator",
            ActionName = "Завершить",
            ConfirmText = "Уверены что завершить?",
            IsSystemAction = false,
            Description = null,
            ToStateId = TripRequestStateEnum.CompletedByCreator.GetEnumGuid()
        });

        uw.AddIfNotExists(
            new DbStateMachineFromStatus
            {
                Id = Guid.Parse("a2ceecc6-4ca3-43e8-bdde-da089c9679a9"),
                IsDeleted = false,
                StateMachine = StateMachineEnum.TripRequest,
                FromStateId = TripRequestStateEnum.CarrierSelected.GetEnumGuid(),
                StateMachineActionId = ga,
                RightCode = TripRequestRights.Completed.GetEnumGuid()
            }
        );

        #endregion
    }
}
