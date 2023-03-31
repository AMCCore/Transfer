using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Utils;
using Transfer.Dal.Entities;

namespace Transfer.Console;

internal static class AddUser
{
    public struct AUser
    { 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string CompanyName { get; set; }

        public string Password { get; set; }
    }


    private static void DoAddUser(this IUnitOfWork uc)
    {
        var u = new DbAccount
        {
            Email = "uriymarshala@gmail.com",
            Password = BCrypt.Net.BCrypt.HashString("HSHQbS4zsvV8a6"),
            PersonData = new DbPersonData
            {
                FirstName = "Юрий",
                LastName = " (Владелец)",
                MiddleName = " ",
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
}

