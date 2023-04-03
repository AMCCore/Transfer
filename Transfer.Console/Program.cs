using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
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

        uc.NotChangeLastUpdateTick = false;

        if(false)
        {
            var ee = uc.GetSet<DbDriver>().ToList();
            foreach(var e in ee)
            {
                e.DateCreated = new DateTime(e.LastUpdateTick);
                uc.SaveChanges();
            }
        }

        if(true)
        {
            var jopa = new Dictionary<Guid?, IList<Guid>>();
            jopa.Add(Guid.Empty, new List<Guid> { Guid.Parse("65CAE23E-2057-40EA-8967-BEFE0CF8E41F"), Guid.Parse("C8567735-785F-4416-9509-AAEC72AFC2EB") });
            //rop@tktransfer.ru
            uc.DoAddUpdateUser(new AddUser.AUser
            {
                Id = Guid.Parse("4ee30e78-f8cd-43ab-bcc5-d11a7fd04608"),
                FirstName = "Руководитель",
                LastName = "Опраторов",
                Password = "3SLtChUsLgT2yh&",
                Email = "rop@tktransfer.ru",
                Rights = jopa,
            });
        }

        if(false)
        {

            //admin
            var aid = Guid.Parse("CC8EFEFA-0D2E-49FF-B982-6E1EDAED2C76");
            uc.AddOrUpdate(new DbAccount { Id = aid, Password = BCrypt.Net.BCrypt.HashString("vU79kFxqz^#@sE3"),  }, (source, destination) => {
                source.Password = destination.Password;
            });

            //top@tktransfer.ru
            aid = Guid.Parse("31FB93BA-E2CD-4D32-95E5-0A6529A61A14");
            uc.AddOrUpdate(new DbAccount { Id = aid, Password = BCrypt.Net.BCrypt.HashString("wZHES#8C3&2!cXE"), }, (source, destination) => {
               source.Password = destination.Password;
            });

            //m1@tktransfer.ru
            uc.DoAddUpdateUser(new AddUser.AUser { 
                Id = Guid.Parse("132c0920-8225-4fd7-b619-d1d3a1d11976"),
                FirstName = "1",
                LastName = "Оператор",
                Password = "2qpzeo75D!9wAeV",
                Email = "m1@tktransfer.ru"
            });

            //m7@tktransfer.ru
            uc.DoAddUpdateUser(new AddUser.AUser
            {
                Id = Guid.Parse("59b2e4cb-a463-4bfa-8d16-1001f16864b9"),
                FirstName = "7",
                LastName = "Оператор",
                Password = "8r@6jwpG@2Rh!CW",
                Email = "m7@tktransfer.ru"
            });

            //m8@tktransfer.ru
            uc.DoAddUpdateUser(new AddUser.AUser
            {
                Id = Guid.Parse("97B45346-84FD-47D2-84E9-E0088333E950"),
                FirstName = "8",
                LastName = "Оператор",
                Password = "zhncV6Ta8hUCs%R",
                Email = "m8@tktransfer.ru"
            });


            //m9@tktransfer.ru
            uc.DoAddUpdateUser(new AddUser.AUser
            {
                Id = Guid.Parse("995D0C58-A1CD-4612-A1BB-95FC5D7A81A9"),
                FirstName = "9",
                LastName = "Оператор",
                Password = "g9tU!8^xZdJVL5U",
                Email = "m9@tktransfer.ru"
            });

            //m10@tktransfer.ru
            uc.DoAddUpdateUser(new AddUser.AUser
            {
                Id = Guid.Parse("5c6dccc7-4ac9-4a5c-a9c5-0d06d683e17b"),
                FirstName = "10",
                LastName = "Оператор",
                Password = "H5&ov4zuM$MX3QE",
                Email = "m10@tktransfer.ru"
            });

        }

        if(false)
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
