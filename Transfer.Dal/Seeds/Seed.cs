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

#if DEBUG
            uw.SetTestEnvironment();
#endif

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
                    Password = BCrypt.Net.BCrypt.HashString("7F4U*3gsR7cHPss"),
                    LastUpdateTick = DateTime.Now.Ticks,
                    DateCreated = DateTime.Now,
                    PersonDataId = Guid.Parse("3EE8A706-3DBD-49A2-8C70-5CD9C2AD5202")
                }
            };

#if !DEBUG

            uw.AddOrUpdate(accounts, (source, destination) =>
            {
                destination.Email = source.Email;
                destination.Password = source.Password;
                destination.PersonDataId = source.PersonDataId;
            });

#endif

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

        private static void SetTestEnvironment(this IUnitOfWork uw)
        {
            var regId = Guid.Parse("6CBCC2D0-D3CE-4515-9E49-464A413D1CEC"); //Краснодарский край

            var o1 = Guid.Parse("66773acf-951b-4552-ac64-f2d4a4b26c5e");
            uw.AddIfNotExists(new DbOrganisation
            {
                Id = o1,
                Name = "Рога и Копыта 1",
                FullName = "ООО Рога и Копыта 1",
                Checked = true,
                DirectorFio = "DirectorFio",
                DirectorPosition = "DirectorPosition",
                Address = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко дом 36, стр 1",
                IsDeleted = false,
                Rating = 99.5,
                City = "Геленджик",
                INN = "0000000001",
                OGRN = "2052931098361",
                LastUpdateTick = DateTime.Now.Ticks,
            });
            uw.AddIfNotExists(new DbOrganisationWorkingArea
            {
                Id = Guid.Parse("85858669-7bbb-4de3-986f-25453b9a887f"),
                OrganisationId = o1,
                RegionId = regId
            });

            var o2 = Guid.Parse("ca291304-1a31-4ddd-8cf2-8e27ea0fa010");
            uw.AddIfNotExists(new DbOrganisation
            {
                Id = o2,
                Name = "Рога и Копыта 2",
                FullName = "ООО Рога и Копыта 2",
                Checked = true,
                DirectorFio = "DirectorFio",
                DirectorPosition = "DirectorPosition",
                Address = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко дом 36, стр 2",
                IsDeleted = false,
                Rating = 99.5,
                City = "Геленджик",
                INN = "0000000002",
                OGRN = "2052931098361",
                LastUpdateTick = DateTime.Now.Ticks
            });
            uw.AddIfNotExists(new DbOrganisationWorkingArea
            {
                Id = Guid.Parse("3ac5ebc4-f7e9-4d76-99a5-970b21929605"),
                OrganisationId = o2,
                RegionId = regId
            });

            var o3 = Guid.Parse("6feeef0c-5b70-4ce6-95cc-8c46b874a09b");
            uw.AddIfNotExists(new DbOrganisation
            {
                Id = o3,
                Name = "Рога и Копыта 3",
                FullName = "ООО Рога и Копыта 3",
                Checked = true,
                DirectorFio = "DirectorFio",
                DirectorPosition = "DirectorPosition",
                Address = "Россия, Краснодарский край, Геленджик, Улица Адмирала Проценко дом 36, стр 3",
                IsDeleted = false,
                Rating = 99.5,
                City = "Геленджик",
                INN = "0000000003",
                OGRN = "2052931098361",
                LastUpdateTick = DateTime.Now.Ticks
            });
            uw.AddIfNotExists(new DbOrganisationWorkingArea
            {
                Id = Guid.Parse("a63b357a-83c2-44b0-91bc-1e94bdf8bc21"),
                OrganisationId = o3,
                RegionId = regId
            });



        }

    }
}
