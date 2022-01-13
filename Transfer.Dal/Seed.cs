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
            uw.SetAccounts();
        }

        private static void SetAccounts(this IUnitOfWork uw)
        {
            var accounts = new List<DbAccount> {
                new DbAccount {
                    Id = adminId,
                    Email = "admin",
                    Password = BCrypt.Net.BCrypt.HashString("admin"),
                    LastUpdateTick = DateTime.Now.Ticks,
                    DateCreated = DateTime.Now
                }
            };

            uw.AddOrUpdate(accounts, (source, destination) => { destination.Email = source.Email; });
        }

        private static void SetRights(this IUnitOfWork uw)
        {
            var rights = new List<DbRight>();
            rights.AddRange(Enum.GetValues(typeof(AdminAccessRights)).Cast<AdminAccessRights>().Select(r => DbRight.CreateForSeed(r)));
            uw.AddIfNotExists(rights);
        }
    }
}
