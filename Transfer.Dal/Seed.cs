using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Dal
{
    public static partial class Seed
    {
        public static void SeedData(this IUnitOfWork uw, bool createDefaultUser = false)
        {
            uw.NotChangeLastUpdateTick = true;
            uw.SetTestAccounts();
        }

        private static void SetTestAccounts(this IUnitOfWork uw)
        {
            var accounts = new List<DbAccount> {
                new DbAccount {
                    Id = Guid.Parse("CC8EFEFA-0D2E-49FF-B982-6E1EDAED2C76"),
                    Email = "admin1@trs.ru",
                    Password = BCrypt.Net.BCrypt.HashString(Guid.NewGuid().ToString()),
                    LastUpdateTick = DateTime.Now.Ticks
                },
                new DbAccount {
                    Id = Guid.Parse("DF50D6F9-65B3-453E-A292-7B49DA2FD01F"),
                    Email = "admin2@trs.ru",
                    Password = BCrypt.Net.BCrypt.HashString(Guid.NewGuid().ToString()),
                    LastUpdateTick = DateTime.Now.Ticks
                },
                new DbAccount {
                    Id = Guid.Parse("1DFF16E6-0EE5-4202-AC1C-43E042905CB3"),
                    Email = "admin3@trs.ru",
                    Password = BCrypt.Net.BCrypt.HashString(Guid.NewGuid().ToString()),
                    LastUpdateTick = DateTime.Now.Ticks
                },
                new DbAccount {
                    Id = Guid.Parse("1690FF9D-99B6-42BF-9E9A-2F41A4B4CB3D"),
                    Email = "admin4@trs.ru",
                    Password = BCrypt.Net.BCrypt.HashString(Guid.NewGuid().ToString()),
                    LastUpdateTick = DateTime.Now.Ticks
                },
                new DbAccount {
                    Id = Guid.Parse("82DB29B9-A492-4E05-A55D-9F22AEBCD829"),
                    Email = "admin5@trs.ru",
                    Password = BCrypt.Net.BCrypt.HashString(Guid.NewGuid().ToString()),
                    LastUpdateTick = DateTime.Now.Ticks
                }
            };

            uw.AddOrUpdate(accounts, (source, destination) => { destination.Email = source.Email; });



        }
    }
}
