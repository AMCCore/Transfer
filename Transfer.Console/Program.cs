using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Transfer.Common.Enums.States;
using Transfer.Dal;
using Transfer.Dal.Entities;

namespace Transfer.Console;

internal class Program
{
    static void Main(string[] args)
    {
        System.Console.WriteLine("Hello World!");

        //using var uc = new UnitOfWork("Data Source=31.31.196.202;Initial Catalog=u0283737_trs;Integrated Security=False;User Id=u0283737_trs;Password=7bcB8$1y;");
        using var uc = new UnitOfWork("Data Source=31.31.196.202;Initial Catalog=u1617627_trs;Integrated Security=False;User Id=u1617627_trs;Password=1Ov8b@o9;");

        using var transaction = uc.Context.Database.BeginTransaction();

        uc.NotChangeLastUpdateTick = true;


        uc.DoAddUser(new AddUser.AUser {
            LastName = "Старкова",
            FirstName = "Екатерина",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "m2@tktransfer.ru"
        });

        uc.DoAddUser(new AddUser.AUser
        {
            LastName = "Баранов",
            FirstName = "Вячеслав",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "m4@tktransfer.ru"
        });

        uc.DoAddUser(new AddUser.AUser
        {
            LastName = "Никифорова",
            FirstName = "Елена",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "m3@tktransfer.ru"
        });

        uc.DoAddUser(new AddUser.AUser
        {
            LastName = "Чурилов",
            FirstName = "Сергей",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "m6@tktransfer.ru"
        });

        uc.DoAddUser(new AddUser.AUser
        {
            LastName = "Марчихина",
            FirstName = "Ольга",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "top@tktransfer.ru"
        });

        uc.DoAddUser(new AddUser.AUser
        {
            LastName = "Федоров",
            FirstName = "Алексей",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "anapa-tktransfer@yandex.ru"
        });

        uc.DoAddUser(new AddUser.AUser
        {
            LastName = "Закирова",
            FirstName = "Мария",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "operator@tktransfer.ru"
        });

        uc.DoAddUser(new AddUser.AUser
        {
            LastName = "Гавриков",
            FirstName = "Александр",
            MiddleName = " ",
            Password = Password.Generate(),
            Email = "m5@tktransfer.ru"
        });

        if (false)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var existingFile = new FileInfo(@"C:\Temp\pppf.xlsx");
            using var package = new ExcelPackage(existingFile);
            var sheet1 = package.Workbook.Worksheets[2];
            int rowCount = sheet1.Dimension.End.Row;
            int i = 1;
            for (int row = 1; row <= rowCount; row++)
            {
                var regionName = sheet1.Cells[row, 1].Value?.ToString();
                if (string.IsNullOrWhiteSpace(regionName))
                    continue;

                if (uc.GetSet<DbRegion>().Any(x => x.Name.ToLower() == regionName.ToLower().Trim()))
                    continue;

                System.Console.WriteLine($"{row}, {i++}, Region:{regionName}");

                uc.AddEntity(new DbRegion
                {
                    Name = regionName.Trim(),
                });
            }

            if (false)
            {
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
                    var regNames = regions.ToLower().Split(new string[] { " и ", "," }, StringSplitOptions.TrimEntries).Select(ss => ss.Trim());
                    var regs = uc.GetSet<DbRegion>().Where(x => regNames.Contains(x.Name.ToLower().Trim())).ToList();
                    if (!regs.Any())
                    {
                        continue;
                    }
                    var phone = sheet1.Cells[row, 5].Value?.ToString().Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("+", string.Empty);
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        phone = $"+7{phone[1..]}";
                    }
                    else
                    {
                        continue;
                    }

                    var email = sheet1.Cells[row, 6].Value?.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(email))
                        continue;

                    var cname = sheet1.Cells[row, 1].Value?.ToString().Trim();

                    System.Console.WriteLine($"{row}, Org:{cname} | {inn}");

                    var o = uc.AddEntity(new DbOrganisation
                    {
                        Name = cname,
                        FullName = sheet1.Cells[row, 3].Value?.ToString().Trim(),
                        Address = sheet1.Cells[row, 4].Value?.ToString().Trim(),
                        FactAddress = sheet1.Cells[row, 4].Value?.ToString().Trim(),
                        Phone = phone,
                        Email = email,
                        INN = inn.Trim(),
                        DirectorFio = "Неизвестен",
                        DirectorPosition = "Директор",
                        State = OrganisationStateEnum.Checked,
                        IsDeleted = false
                    });

                    foreach (var r in regs)
                    {
                        uc.AddEntity(new DbOrganisationWorkingArea
                        {
                            Organisation = o,
                            Region = r
                        });
                    }

                    var u = uc.DoAddUser(new AddUser.AUser
                    {
                        CompanyName = cname,
                        FirstName = "Неизвестен",
                        Phone = phone,
                        Email = email
                    });

                    uc.AddEntity(new DbOrganisationAccount { Organisation = o, AccountType = Common.Enums.OrganisationAccountTypeEnum.Director, AccountId = u });
                }

            }

            if (false)
            {
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
                        model = "Неизвестная";

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

                    System.Console.WriteLine($"{row}, tc:{make} | {number.ToUpper()}");

                    uc.AddEntity(new DbBus
                    {
                        Make = make,
                        LicenseNumber = number.ToUpper(),
                        Model = model,
                        Yaer = Convert.ToInt32(year),
                        PeopleCopacity = capacityNum,
                        Organisation = org,
                        IsDeleted = false,
                        State = BusStateEnum.Checked
                    });

                }

            }
        }

        transaction.Commit();
        return;

        var admin = uc.GetSet<DbAccount>().FirstOrDefault(x => x.Id == Guid.Parse("CC8EFEFA-0D2E-49FF-B982-6E1EDAED2C76"));
        admin.Password = BCrypt.Net.BCrypt.HashString("jopa");
        uc.SaveChanges();
    }
}
