using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Extensions;
using Transfer.Dal.Entities;

namespace Transfer.Console;

internal static class AddUser
{
    public class AUser
    { 
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string CompanyName { get; set; }

        public string Password { get; set; }

        public IDictionary<Guid?, IList<Guid>> Rights { get; set; } = new Dictionary<Guid?, IList<Guid>>();
    }


    public static Guid DoAddUser(this IUnitOfWork uc, AUser user)
    {
        System.Console.WriteLine($"{user.FirstName} {user.LastName} {user.Email} pass:{user.Password}");

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            var pwrd = Password.Generate();
            user.Password = pwrd;
        }

        var u = new DbAccount
        {
            Email = user.Email,
            Password = BCrypt.Net.BCrypt.HashString(user.Password),
            PersonData = new DbPersonData
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                DocumentSeries = " ",
                DocumentNumber = " ",
                DocumentIssurer = " ",
                DocumentSubDivisionCode = " ",
                DocumentDateOfIssue = DateTime.MinValue,
                RegistrationAddress = " ",
            },
            LastUpdateTick = -1
        };

        uc.AddEntity(u);

        return u.Id;
    }

    public static void DoAddUpdateUser(this IUnitOfWork uc, AUser user)
    {
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            var pwrd = Password.Generate();
            user.Password = pwrd;
        }

        if(user.Id.IsNullOrEmpty())
        {
            user.Id = Guid.NewGuid();
        }

        uc.AddOrUpdate(new DbAccount { Id = user.Id, Password = BCrypt.Net.BCrypt.HashString(user.Password), Email = user.Email, }, (source, destination) => {
            source.Password = destination.Password;
            source.Email = destination.Email;
        });

        var pd = uc.AddEntity(new DbPersonData
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName ?? " ",
            DocumentSeries = " ",
            DocumentNumber = " ",
            DocumentIssurer = " ",
            DocumentSubDivisionCode = " ",
            DocumentDateOfIssue = DateTime.MinValue,
            RegistrationAddress = " ",
        });

        var ur = uc.GetSet<DbAccountRight>().Where(x => x.AccountId == user.Id).ToList();

        uc.DeleteList(ur);

        var rr = new List<DbAccountRight>();
        foreach(var r in user.Rights)
        {
            foreach(var rrr in r.Value)
            {
                uc.AddEntity(new DbAccountRight { OrganisationId = r.Key.IsNullOrEmpty() ? null : r.Key, AccountId = user.Id, RightId = rrr });
            }
        }

        var uuu = uc.GetSet<DbAccount>().First(x => x.Id == user.Id);
        uuu.PersonDataId = pd.Id;

        uc.SaveChanges();
        
        
    }
}

