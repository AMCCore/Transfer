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

namespace Transfer.Dal.Seeds
{
    public static class MainSeed
    {
        internal static readonly Guid adminId = Guid.Parse("CC8EFEFA-0D2E-49FF-B982-6E1EDAED2C76");

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

        private static void ssdsdsdsSetTripRequestUserStatets(this IUnitOfWork uw)
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



            // --> отменён
            var ga = Guid.Parse("94D3666E-70C2-4F85-B5A1-A4733A0A1AAD");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "cancel",
                ActionName = "Отменить",
                Description = null,
                IsSystemAction = false,
                ToStateId = Guid.Parse("92B66995-F591-4C6F-90B2-F222B9CEAD2D"),

            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("84A58E87-8B2F-42DF-8086-4367F7E9DD87"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("66FD6072-3F96-46B8-87F1-E29D915B1C34"),
                    StateMachineActionId = ga
                });

            // --> просрочен
            ga = Guid.Parse("9A1CDF22-B861-4246-AF2E-DC27237AB41B");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "overdue",
                ActionName = "Просрочить",
                IsSystemAction = true,
                Description = null,
                ToStateId = Guid.Parse("55453A63-978B-4E4C-B94B-F1606E534AED")
            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("B52C828C-2F1D-4C99-BFE8-2E73EC2476F5"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("66FD6072-3F96-46B8-87F1-E29D915B1C34"),
                    StateMachineActionId = ga
                },
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("84A58E87-8B2F-42DF-8086-4367F7E9DD87"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("19AA45E1-6CEF-4F80-B058-39639DC9CED1"),
                    StateMachineActionId = ga
                }
            );

            // --> перевозчик выбран
            ga = Guid.Parse("00AE0C75-94F1-447E-866D-23DD149C60C0");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "chooseCarrier",
                ActionName = "Выбрать перевозчика",
                IsSystemAction = false,
                Description = null,
                ToStateId = Guid.Parse("19AA45E1-6CEF-4F80-B058-39639DC9CED1")
            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("D53B957C-E023-44DA-8C81-15E126B63ED7"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("66FD6072-3F96-46B8-87F1-E29D915B1C34"),
                    StateMachineActionId = ga
                },
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("56130327-0D8A-4961-ABDE-DE5801D99365"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("A005D7AC-9B2D-4234-A78A-0FDCF3659C5F"),
                    StateMachineActionId = ga
                }
            );

            // --> изменение перевозчика
            ga = Guid.Parse("7F4EA374-394E-4F3F-B25D-CE6018B14166");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "changeCarrier",
                ActionName = "Сменить перевозчика",
                IsSystemAction = false,
                Description = null,
                ToStateId = Guid.Parse("A005D7AC-9B2D-4234-A78A-0FDCF3659C5F")
            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("6929D630-1DD3-4CF8-8296-2A4324DD0565"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("19AA45E1-6CEF-4F80-B058-39639DC9CED1"),
                    StateMachineActionId = ga
                }
            );

            // --> выполнен
            ga = Guid.Parse("568BC33D-FF44-4939-9786-D48DDFE8B966");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "completed",
                ActionName = "Выполнить",
                IsSystemAction = false,
                Description = null,
                ToStateId = Guid.Parse("A04D4A33-344D-40DF-BCD8-0B36C32DBAC7")
            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("81DE65CE-F563-476A-B088-9880D6B0F47B"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("19AA45E1-6CEF-4F80-B058-39639DC9CED1"),
                    StateMachineActionId = ga
                }
            );

            // --> завершен
            ga = Guid.Parse("568BC33D-FF44-4939-9786-D48DDFE8B966");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "completed",
                ActionName = "Завершить",
                IsSystemAction = false,
                Description = null,
                ToStateId = Guid.Parse("10410CA1-47E7-4DD3-99B5-338A39AF71B1")
            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("FB76E480-1896-474B-AD91-AD4EA2E298E3"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("A04D4A33-344D-40DF-BCD8-0B36C32DBAC7"),
                    StateMachineActionId = ga
                }
            );

            // --> Завершен без подтверждения
            ga = Guid.Parse("35580EB4-6A8A-4B97-BA44-2C5193A3DD4E");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "completedWithoutConfirm",
                ActionName = "Завершить без подтверждения",
                IsSystemAction = false,
                Description = null,
                ToStateId = Guid.Parse("EF160963-32C5-4734-9CD4-4FB7B73E01F5")
            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("C5C56A00-B0F2-43F1-99FA-2015C72020F1"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("A04D4A33-344D-40DF-BCD8-0B36C32DBAC7"),
                    StateMachineActionId = ga
                }
            );

            // --> архивные
            ga = Guid.Parse("AA851CAF-4346-4126-8501-0D6EB74FE4FD");
            uw.AddIfNotExists(new DbStateMachineAction
            {
                Id = ga,
                IsDeleted = false,
                StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                ActionCode = "archive",
                ActionName = "Переместить в архив",
                IsSystemAction = true,
                Description = null,
                ToStateId = Guid.Parse("950C380C-55E2-4D97-82A7-CCF3F8A87D72")
            });

            uw.AddIfNotExists(
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("32487569-C8F7-444C-BAA6-6D3821254A21"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("92B66995-F591-4C6F-90B2-F222B9CEAD2D"),
                    StateMachineActionId = ga
                },
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("552FCCC0-A248-4AA7-AF4B-0C33A5F6EA5A"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("55453A63-978B-4E4C-B94B-F1606E534AED"),
                    StateMachineActionId = ga
                },
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("C773DFF2-C0D9-40E1-907A-AC09E12E687D"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("EF160963-32C5-4734-9CD4-4FB7B73E01F5"),
                    StateMachineActionId = ga
                },
                new DbStateMachineFromStatus
                {
                    Id = Guid.Parse("C9FE2A90-8DE9-4D5B-A5F0-39F52D8DAB9A"),
                    IsDeleted = false,
                    StateMachine = Common.Enums.States.StateMachineEnum.TripRequest,
                    FromStateId = Guid.Parse("10410CA1-47E7-4DD3-99B5-338A39AF71B1"),
                    StateMachineActionId = ga
                }
            );
        }
    }
}
