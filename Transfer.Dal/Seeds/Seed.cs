using System;
using System.Collections.Generic;
using System.Linq;
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
            uw.SetTripRequestUserStates();


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
    }
}
