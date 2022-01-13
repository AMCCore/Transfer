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
            var accounts = new List<DbAccount> {
                new DbAccount {
                    Id = adminId,
                    Email = "admin",
                    Password = BCrypt.Net.BCrypt.HashString("admin"),
                    LastUpdateTick = DateTime.Now.Ticks,
                    DateCreated = DateTime.Now,
                    AccountRights = new List<DbAccountRight>()
                    {
                        new DbAccountRight
                        {
                            Id = Guid.NewGuid(),
                            RightId = AdminAccessRights.IsAdmin.GetEnumGuid()
                        }
                    }
                }
            };

            uw.AddOrUpdate(accounts, (source, destination) => { destination.Email = source.Email; });
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
                    LastUpdateTick = DateTime.Now.Ticks
                }

            };
            uw.AddIfNotExists(prgs);
        }
    }
}
