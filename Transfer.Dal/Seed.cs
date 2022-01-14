using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
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

            uw.SetTestOrganisations();
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

            uw.AddOrUpdate(accounts, (source, destination) => { destination.Email = source.Email;
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
                    Address = "Россия, Краснодарскийкрай, Геленджик, УлицаАдмиралаПроценко дом 36, стр 5",
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
                    Address = "Россия, Краснодарскийкрай, Геленджик, УлицаАдмиралаПроценко дом 36, стр 5",
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
    }
}
