using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums;
using Transfer.Common.Enums.AccessRights;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Dal
{
    public static partial class Seed
    {
        private static Guid adminId = Guid.Parse("CC8EFEFA-0D2E-49FF-B982-6E1EDAED2C76");

        public static void SeedData(this IUnitOfWork uw, bool createDefaultUser = false)
        {
            uw.NotChangeLastUpdateTick = true;
            uw.SetRights();
            uw.SetAccounts();
            uw.SetTripOptions();
            uw.SetRequestTripIds();
            uw.SetTripRequestUserStatets();


            //uw.SetTestOrganisations();
            //uw.SetTestRequestTrips();
        }

        private static void SetAccounts(this IUnitOfWork uw)
        {
            var pd = new List<DbPersonData> {
            new DbPersonData {
                Id = Guid.Parse("3EE8A706-3DBD-49A2-8C70-5CD9C2AD5202"),
                FirstName = "Виктор",
                LastName = "Иванов",
                MiddleName = "Онотольевич",
                BirthDate = new DateTime(1900, 3, 6),
                IsMale = true,
                DocumentSeries = string.Empty,
                DocumentNumber = string.Empty,
                DocumentSubDivisionCode = string.Empty,
                DocumentIssurer = string.Empty,
                DocumentDateOfIssue = DateTime.MinValue,
                RegistrationAddress = string.Empty
            }};
            uw.AddIfNotExists(pd);


            var accounts = new List<DbAccount> {
                new DbAccount {
                    Id = adminId,
                    Email = "admin",
                    Password = BCrypt.Net.BCrypt.HashString("admin"),
                    LastUpdateTick = DateTime.Now.Ticks,
                    DateCreated = DateTime.Now,
                    PersonDataId = Guid.Parse("3EE8A706-3DBD-49A2-8C70-5CD9C2AD5202")
                }
            };

            uw.AddOrUpdate(accounts, (source, destination) =>
            {
                destination.Email = source.Email;
                destination.Password = source.Password;
                destination.PersonDataId = source.PersonDataId;
            });

            var accr = new List<DbAccountRight>
            {
                new DbAccountRight
                {
                    Id = Guid.Parse("5164F65D-AD28-454A-99A5-B630996C1474"),
                    AccountId = adminId,
                    RightId = AdminAccessRights.IsAdmin.GetEnumGuid(),
                    LastUpdateTick = DateTime.Now.Ticks,
                    OrganisationId = null
                }
            };
            uw.AddIfNotExists(accr);

        }

        private static void SetRights(this IUnitOfWork uw)
        {
            var rights = new List<DbRight>();
            rights.AddRange(Enum.GetValues(typeof(AdminAccessRights)).Cast<AdminAccessRights>().Select(r => DbRight.CreateForSeed(r)));
            rights.AddRange(Enum.GetValues(typeof(TripRequestRights)).Cast<TripRequestRights>().Select(r => DbRight.CreateForSeed(r)));
            rights.AddRange(Enum.GetValues(typeof(AccountAccessRights)).Cast<AccountAccessRights>().Select(r => DbRight.CreateForSeed(r)));
            uw.AddOrUpdate(rights, (source, destination) => { destination.Name = source.Name; });
        }

        private static void SetTripOptions(this IUnitOfWork uw)
        {
            var rights = new List<DbTripOption>();
            rights.AddRange(Enum.GetValues(typeof(TripOptionsEnum)).Cast<TripOptionsEnum>().Select(r => DbTripOption.CreateForSeed(r)));
            uw.AddOrUpdate(rights, (source, destination) => { destination.Name = source.Name; });
        }

        private static void SetTestOrganisations(this IUnitOfWork uw)
        {
            var prgs = new List<DbOrganisation>
            {
                new DbOrganisation
                {
                    Id = Guid.Parse("3F604C4E-5F64-4A6E-9A0E-489FC9EDCE3F"),
                    Name = "Рога и Копыта",
                    FullName = "ООО Рога и Копыта",
                    Checked = true,
                    DirectorFio = "DirectorFio",
                    DirectorPosition = "DirectorPosition",
                    Address = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко дом 36, стр 5",
                    IsDeleted = false,
                    Rating = 99.5,
                    City = "Геленджик",
                    INN = "5390730577",
                    OGRN = "2052931098361",
                    LastUpdateTick = DateTime.Now.Ticks
                },
                new DbOrganisation
                {
                    Id = Guid.Parse("77B414CD-F165-448B-B8A1-FCDBD14EFA7C"),
                    Name = "Копыта и Копыта",
                    FullName = "ООО Копыта и Копыта",
                    Checked = true,
                    DirectorFio = "DirectorFio",
                    DirectorPosition = "DirectorPosition",
                    Address = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко дом 36, стр 5",
                    IsDeleted = false,
                    Rating = 99.5,
                    City = "Геленджик",
                    INN = "8799526752",
                    OGRN = "7073663895199",
                    LastUpdateTick = DateTime.Now.Ticks
                }

            };
            uw.AddIfNotExists(prgs);
        }

        private static void SetTestRequestTrips(this IUnitOfWork uw)
        {
            var tr = new List<DbTripRequest> {
                new DbTripRequest {
                    Id = Guid.Parse("911C29AC-A98D-4102-9410-DC03D1727D3A"),
                    AddressFrom = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко, дом 36, стр 5",
                    AddressTo = "Россия, Краснодарский край, Майкоп, Улица Адмирала Проценко, дом 1",
                    ContactEmail = string.Empty,
                    ContactFio = string.Empty,
                    ContactPhone = string.Empty,
                    TripDate = new DateTime(2022, 3, 1),
                    Passengers = 10,
                },
                new DbTripRequest {
                    Id = Guid.Parse("2EFF1D50-B439-4D61-9253-694DF86D10A1"),
                    ContactEmail = string.Empty,
                    ContactFio = string.Empty,
                    ContactPhone = string.Empty,
                    AddressFrom = "Россия, Крым, Ялта, Улица Сталина, дом 2, стр 5",
                    AddressTo = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко, дом 1",
                    TripDate = new DateTime(2022, 3, 4),
                    Passengers = 15,
                },
                new DbTripRequest {
                    Id = Guid.Parse("0DDF23B0-2C56-4ED0-AC05-659AE66D104C"),
                    ContactEmail = string.Empty,
                    ContactFio = string.Empty,
                    ContactPhone = string.Empty,
                    AddressTo = "Россия, Крым, Ялта, Улица Сталина дом 2, стр 5",
                    AddressFrom = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко, дом 1",
                    TripDate = new DateTime(2022, 3, 6),
                    Passengers = 15,
                },
                new DbTripRequest {
                    Id = Guid.Parse("CC34A40C-DAF3-4768-823C-9C56B653B4A1"),
                    ContactEmail = string.Empty,
                    ContactFio = string.Empty,
                    ContactPhone = string.Empty,
                    AddressFrom = "Россия, Москва, Ломоносовский проспект, дом 3, к1",
                    AddressTo = "Россия, Ханты-Мансийский автономный округ, Когалым, Улица Молодёжная, дом 19",
                    TripDate = new DateTime(2022, 2, 1),
                    Passengers = 1,
                },
                new DbTripRequest {
                    Id = Guid.Parse("64854EBF-A65D-410C-9988-F5F81399A88F"),
                    ContactEmail = string.Empty,
                    ContactFio = string.Empty,
                    ContactPhone = string.Empty,
                    AddressFrom = "Россия, Ханты-Мансийский автономный округ, Когалым, Улица Молодёжная, дом 19",
                    AddressTo = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко дом 1",
                    TripDate = new DateTime(2022, 5, 1),
                    Passengers = 25,
                },
            };

            uw.AddIfNotExists(tr);

            var to = new List<DbTripRequestOption> {
                new DbTripRequestOption {
                    Id = Guid.Parse("CF8B46FB-117F-4A81-83CC-E6DC4D208142"),
                    TripRequestId = Guid.Parse("2EFF1D50-B439-4D61-9253-694DF86D10A1"),
                    TripOptionId = TripOptionsEnum.CardPayment.GetEnumGuid()
                },
                new DbTripRequestOption {
                    Id = Guid.Parse("2A4673EA-E9F7-4280-861A-DB1BBA0A8962"),
                    TripRequestId = Guid.Parse("2EFF1D50-B439-4D61-9253-694DF86D10A1"),
                    TripOptionId = TripOptionsEnum.ChildTrip.GetEnumGuid()
                },
                new DbTripRequestOption {
                    Id = Guid.Parse("44597BCF-B59D-4E39-8DAB-9BA6CAD60DAD"),
                    TripRequestId = Guid.Parse("2EFF1D50-B439-4D61-9253-694DF86D10A1"),
                    TripOptionId = TripOptionsEnum.TripGuide.GetEnumGuid()
                },
                new DbTripRequestOption {
                    Id = Guid.Parse("702CFBE8-CA87-411C-BF18-AB78D93E0889"),
                    TripRequestId = Guid.Parse("CC34A40C-DAF3-4768-823C-9C56B653B4A1"),
                    TripOptionId = TripOptionsEnum.TripGuide.GetEnumGuid()
                },
                new DbTripRequestOption {
                    Id = Guid.Parse("5B9A34E3-E741-497C-BE29-FE2AB8EF6FAB"),
                    TripRequestId = Guid.Parse("CC34A40C-DAF3-4768-823C-9C56B653B4A1"),
                    TripOptionId = TripOptionsEnum.CashPayment.GetEnumGuid()
                },

            };
            uw.AddIfNotExists(to);

        }

        private static void SetRequestTripIds(this IUnitOfWork uw)
        {
            var reqs = uw.GetSet<DbTripRequest>().Where(x => !x.Identifiers.Any()).OrderBy(x => x.DateCreated).Select(x => x.Id).ToList();
            foreach (var req in reqs)
            {
                uw.AddEntity(new DbTripRequestIdentifier
                {
                    Id = Guid.NewGuid(),
                    TripRequestId = req,
                    LastUpdateTick = DateTime.Now.Ticks
                });
            }
        }

        private static void SetTripRequestUserStatets(this IUnitOfWork uw)
        {
            var tripRequestUserStatets = new List<DbStateMachineState>
            {
                new DbStateMachineState {
                    Id = Guid.Parse("66FD6072-3F96-46B8-87F1-E29D915B1C34"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Новый заказ",
                    Description = "Cбор предложений/откликов перевозчиков"
                },
                new DbStateMachineState {
                    Id = Guid.Parse("19AA45E1-6CEF-4F80-B058-39639DC9CED1"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Перевозчик выбран",
                    Description = "Заключение договора"
                },
                new DbStateMachineState {
                    Id = Guid.Parse("A04D4A33-344D-40DF-BCD8-0B36C32DBAC7"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Выполнен",
                    Description = "Ожидание подтверждения"
                },
                new DbStateMachineState {
                    Id = Guid.Parse("10410CA1-47E7-4DD3-99B5-338A39AF71B1"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Завершен",
                    Description = null
                },
                new DbStateMachineState {
                    Id = Guid.Parse("92B66995-F591-4C6F-90B2-F222B9CEAD2D"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Отменен",
                    Description = null
                },
                new DbStateMachineState {
                    Id = Guid.Parse("55453A63-978B-4E4C-B94B-F1606E534AED"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Просрочен",
                    Description = null
                },
                new DbStateMachineState {
                    Id = Guid.Parse("A005D7AC-9B2D-4234-A78A-0FDCF3659C5F"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Изменение перевозчика",
                    Description = null
                },
                new DbStateMachineState {
                    Id = Guid.Parse("EF160963-32C5-4734-9CD4-4FB7B73E01F5"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Завершен без подтверждения",
                    Description = null
                },
                new DbStateMachineState {
                    Id = Guid.Parse("950C380C-55E2-4D97-82A7-CCF3F8A87D72"),
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    IsDeleted = false,
                    Name = "Перемещен в архив",
                    Description = null
                },
            };
            uw.AddIfNotExists(tripRequestUserStatets);

            //var tripRequestUserStatets = new List<DbStateMachineState> {
            //    //новая поездка
            //    new DbStateMachineState {
            //        Id = Guid.Parse("0EA2A18A-D7E7-4725-9BB4-42816C855040"),
            //        StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
            //        StateFrom = Common.Enums.States.TripRequestStateEnum.Active.GetEnumGuid(),
            //        StateTo = Common.Enums.States.TripRequestStateEnum.Canceled.GetEnumGuid(),
            //        ConfirmText = "Вы уверены что хотите отменить запрос?",
            //        ButtonName = "Отменить",
            //        Description = $"{Common.Enums.States.TripRequestStateEnum.Active.GetEnumDescription()} -> {Common.Enums.States.TripRequestStateEnum.Canceled.GetEnumDescription()}",
            //        UseBySystem = false,
            //        UseByAuthorized = true,
            //        UseByOrganisation = false,
            //        UseByOwner = true,
            //    },
            //    //выбор перевозчика - переход не из машины статусов

            //    //перевозчик выбран
            //    new DbStateMachineState {
            //        Id = Guid.Parse("9346C55E-105C-4F46-ACB4-346699DE7B4F"),
            //        StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
            //        StateFrom = Common.Enums.States.TripRequestStateEnum.CarrierSelected.GetEnumGuid(),
            //        StateTo = Common.Enums.States.TripRequestStateEnum.Completed.GetEnumGuid(),
            //        ButtonName = "Завершить",
            //        ConfirmText = "Вы уверены что хотите завершить поездку?",
            //        Description = $"{Common.Enums.States.TripRequestStateEnum.CarrierSelected.GetEnumDescription()} -> {Common.Enums.States.TripRequestStateEnum.Completed.GetEnumDescription()}",
            //        UseBySystem = false,
            //        UseByAuthorized = true,
            //        UseByOrganisation = false,
            //        UseByOwner = true,
            //    },
            //    new DbStateMachineState {
            //        Id = Guid.Parse("E037B97D-BAC7-42FC-AC90-81DC33952B53"),
            //        StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
            //        StateFrom = Common.Enums.States.TripRequestStateEnum.CarrierSelected.GetEnumGuid(),
            //        StateTo = Common.Enums.States.TripRequestStateEnum.Canceled.GetEnumGuid(),
            //        ButtonName = "Отменить",
            //        ConfirmText = "Вы уверены что хотите отменить запрос?",
            //        Description = $"{Common.Enums.States.TripRequestStateEnum.CarrierSelected.GetEnumDescription()} -> {Common.Enums.States.TripRequestStateEnum.Canceled.GetEnumDescription()}",
            //        UseBySystem = false,
            //        UseByAuthorized = true,
            //        UseByOrganisation = false,
            //        UseByOwner = true,
            //    },

            //};

            //var add = new List<DbStateMachineState> {
            //    new DbStateMachineState {
            //        Id = Guid.Parse("8F5042C5-19C7-4F11-9581-96661F054FE6"),
            //        StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
            //        StateFrom = Common.Enums.States.TripRequestStateEnum.Active.GetEnumGuid(),
            //        StateTo = Common.Enums.States.TripRequestStateEnum.Overdue.GetEnumGuid(),
            //        Description = $"{Common.Enums.States.TripRequestStateEnum.Active.GetEnumDescription()} -> {Common.Enums.States.TripRequestStateEnum.Overdue.GetEnumDescription()}",
            //        UseBySystem = true,
            //        UseByAuthorized = false,
            //        UseByOrganisation = false,
            //        UseByOwner = false,
            //    },

            //    //отменённая поездка
            //    new DbStateMachineState {
            //        Id = Guid.Parse("757EAF33-2E50-41D8-9F1D-88297D69E357"),
            //        StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
            //        StateFrom = Common.Enums.States.TripRequestStateEnum.Canceled.GetEnumGuid(),
            //        StateTo = Common.Enums.States.TripRequestStateEnum.Archived.GetEnumGuid(),
            //        Description = $"{Common.Enums.States.TripRequestStateEnum.Canceled.GetEnumDescription()} -> {Common.Enums.States.TripRequestStateEnum.Archived.GetEnumDescription()}",
            //        UseBySystem = true,
            //        UseByAuthorized = false,
            //        UseByOrganisation = false,
            //        UseByOwner = false,
            //    },
            //    //просроченная поездка
            //    new DbStateMachineState {
            //        Id = Guid.Parse("3BA46311-1C13-4454-88BE-10C8089F99F8"),
            //        StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
            //        StateFrom = Common.Enums.States.TripRequestStateEnum.Overdue.GetEnumGuid(),
            //        StateTo = Common.Enums.States.TripRequestStateEnum.Archived.GetEnumGuid(),
            //        Description = $"{Common.Enums.States.TripRequestStateEnum.Overdue.GetEnumDescription()} -> {Common.Enums.States.TripRequestStateEnum.Archived.GetEnumDescription()}",
            //        UseBySystem = true,
            //        UseByAuthorized = false,
            //        UseByOrganisation = false,
            //        UseByOwner = false,
            //    },
            //};

            //uw.AddIfNotExists(tripRequestUserStatets);

            //uw.AddOrUpdate(tripRequestUserStatets, (source, destination) =>
            //{
            //    destination.StateFrom = source.StateFrom;
            //    destination.StateTo = source.StateTo;
            //    destination.UseByOwner = source.UseByOwner;
            //    destination.UseByAuthorized = source.UseByAuthorized;
            //    destination.UseByOrganisation = source.UseByOrganisation;
            //    destination.UseBySystem = source.UseBySystem;
            //    destination.Description = source.Description;
            //    destination.ButtonName = source.ButtonName;
            //    destination.ConfirmText = source.ConfirmText;
            //});
        }
    }
}
