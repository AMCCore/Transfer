using System;
using System.Linq;
using Transfer.Dal;
using Transfer.Dal.Entities;

namespace Transfer.Console;

internal class Program
{
    static void Main(string[] args)
    {
        System.Console.WriteLine("Hello World!");

        using var uc = new UnitOfWork("Data Source=31.31.196.202;Initial Catalog=u0283737_trs;Integrated Security=False;User Id=u0283737_trs;Password=7bcB8$1y;");
        //using var uc = new UnitOfWork("Data Source=31.31.196.202;Initial Catalog=u1617627_trs;Integrated Security=False;User Id=u1617627_trs;Password=1Ov8b@o9;");

        var admin = uc.GetSet<DbAccount>().FirstOrDefault(x => x.Id == Guid.Parse("CC8EFEFA-0D2E-49FF-B982-6E1EDAED2C76"));
        admin.Password = BCrypt.Net.BCrypt.HashString("jopa");
        uc.SaveChanges();


        return;

        var u = new DbAccount
        {
            Email = "sonarv@mail.ru",
            Password = BCrypt.Net.BCrypt.HashString("jopashnik"),
            PersonData = new DbPersonData { 
                FirstName = "Виктор",
                LastName = "Бирюков (Админ)",
                MiddleName = "Сергеевич",
                DocumentSeries = " ",
                DocumentNumber = " ",
                DocumentIssurer = " ",
                DocumentSubDivisionCode = " ",
                DocumentDateOfIssue = DateTime.MinValue,
                RegistrationAddress = " ",
            }
        };

        uc.AddEntity(u);
    }
}
