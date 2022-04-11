using OfficeOpenXml;
using System;
using System.IO;
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

        using var transaction = uc.Context.Database.BeginTransaction();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var existingFile = new FileInfo(@"E:\temp\PlatformTransports.xlsx");
        using var package = new ExcelPackage(existingFile);
        var sheet1 = package.Workbook.Worksheets[2];
        int rowCount = sheet1.Dimension.End.Row;
        for (int row = 2; row <= rowCount; row++)
        {
            var regionName = sheet1.Cells[row, 2].Value?.ToString();
            if (string.IsNullOrWhiteSpace(regionName))
                continue;

            if (uc.GetSet<DbRegion>().Any(x => x.Name.ToLower() == regionName.ToLower().Trim()))
                continue;

            uc.AddEntity(new DbRegion { 
                Name = regionName.Trim(),
            });
        }

        sheet1 = package.Workbook.Worksheets[0];
        rowCount = sheet1.Dimension.End.Row;
        for (int row = 2; row <= rowCount; row++)
        {
            //инн
            var inn = sheet1.Cells[row, 7].Value?.ToString();
            if (string.IsNullOrWhiteSpace(inn))
                continue;

            if (uc.GetSet<DbOrganisation>().Any(x => x.INN == inn.Trim()))
                continue;

            var regions = sheet1.Cells[row, 2].Value?.ToString();
            var regNames = regions.ToLower().Split(" и ").Select(ss => ss.Trim());
            var regs = uc.GetSet<DbRegion>().Where(x => regNames.Contains(x.Name.ToLower().Trim())).ToList();
            var phone = sheet1.Cells[row, 5].Value?.ToString().Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("+", string.Empty);
            if(!string.IsNullOrWhiteSpace(phone))
            {
                phone = $"+7{phone[1..]}";
            }
            else
            {
                phone = "unknown";
            }

            var o = uc.AddEntity(new DbOrganisation { 
                Name = sheet1.Cells[row, 1].Value?.ToString().Trim(),
                FullName = sheet1.Cells[row, 3].Value?.ToString().Trim(),
                Address = sheet1.Cells[row, 4].Value?.ToString().Trim(),
                FactAddress = " ",
                Phone = phone,
                Email = sheet1.Cells[row, 6].Value?.ToString().Trim() ?? "unknown",
                INN = inn.Trim(),
                DirectorFio = "Неизвестен",
                DirectorPosition = "Директор",
                State = Common.Enums.OrganisationStateEnum.Checked,
                IsDeleted = false
            });

            foreach(var r in regs)
            {
                uc.AddEntity(new DbOrganisationWorkingArea { 
                    Organisation = o,
                    Region = r
                });
            }



        }

        sheet1 = package.Workbook.Worksheets[1];
        rowCount = sheet1.Dimension.End.Row;
        for (int row = 2; row <= rowCount; row++)
        {
            var orgName = sheet1.Cells[row, 1].Value?.ToString();
            var org = uc.GetSet<DbOrganisation>().FirstOrDefault(x => x.Name == orgName);
            if (org == null)
                continue;

            var make = sheet1.Cells[row, 2].Value?.ToString();
            if (string.IsNullOrWhiteSpace(make))
                continue;

            var model = sheet1.Cells[row, 3].Value?.ToString();
            if (string.IsNullOrWhiteSpace(model))
                continue;

            var number = sheet1.Cells[row, 4].Value?.ToString();
            if (string.IsNullOrWhiteSpace(number))
                continue;

            number = number.Replace(" ", string.Empty);
            if (uc.GetSet<DbBus>().Any(x => !x.IsDeleted && x.LicenseNumber.ToLower() == number.ToLower()))
                continue;

            var year = sheet1.Cells[row, 5].Value?.ToString();
            if (string.IsNullOrWhiteSpace(year))
                continue;

            var capacity = sheet1.Cells[row, 6].Value?.ToString();
            int.TryParse(capacity, out int capacityNum);


            uc.AddEntity(new DbBus { 
                Make = make,
                LicenseNumber = number.ToUpper(),
                Model = model,
                Yaer = Convert.ToInt32(year),
                PeopleCopacity = capacityNum,
                Organisation = org,
                IsDeleted = false,
                State = Common.Enums.BusStateEnum.Checked
           });

        }


        transaction.Commit();
        return;

        var admin = uc.GetSet<DbAccount>().FirstOrDefault(x => x.Id == Guid.Parse("CC8EFEFA-0D2E-49FF-B982-6E1EDAED2C76"));
        admin.Password = BCrypt.Net.BCrypt.HashString("jopa");
        uc.SaveChanges();

        

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
