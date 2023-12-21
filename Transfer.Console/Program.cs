using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;
using Transfer.Dal;
using Transfer.Dal.Entities;

namespace Transfer.Console;

internal class Program
{
    static int GetOrAddInfSys(SqlTransaction tran, string InfSysName, string ProductName)
    {
        using var command1 = new SqlCommand("SELECT TOP 1 a.[Id] FROM [dbo].[DR_InfSys] a INNER JOIN [dbo].[DR_Product] b ON b.[Id] = a.[ProductId] WHERE a.[Name] = @name1 AND b.[Name] = @name2", tran.Connection);
        command1.Parameters.AddWithValue("name1", InfSysName);
        command1.Parameters.AddWithValue("name2", ProductName);
        command1.Transaction = tran;
        var res = command1.ExecuteScalar();
        if (res == null)
        {
            var productId = GetOrAddProduct(tran, ProductName);
            using var command2 = new SqlCommand("INSERT INTO [dbo].[DR_InfSys] ([Name], [ProductId]) VALUES (@name, @prid); Select SCOPE_IDENTITY();", tran.Connection);
            command2.Parameters.AddWithValue("name", InfSysName);
            command2.Parameters.AddWithValue("prid", productId);
            command2.Transaction = tran;
            var res2 = command2.ExecuteScalar();
            return Convert.ToInt32(res2);
        }
        return Convert.ToInt32(res);
    }

    static int GetOrAddProduct(SqlTransaction tran, string Name)
    {
        using var command1 = new SqlCommand("SELECT TOP 1 [Id] FROM [dbo].[DR_Product] WHERE [Name] = @name", tran.Connection);
        command1.Parameters.AddWithValue("name", Name);
        command1.Transaction = tran;
        var res = command1.ExecuteScalar();
        if(res == null)
        {
            using var command2 = new SqlCommand("INSERT INTO [dbo].[DR_Product] ([Name]) VALUES (@name); Select SCOPE_IDENTITY();", tran.Connection);
            command2.Parameters.AddWithValue("name", Name);
            command2.Transaction = tran;
            var res2 = command2.ExecuteScalar();
            return Convert.ToInt32(res2);
        }

        return Convert.ToInt32(res);
    }

    static void Main(string[] args)
    {
        System.Console.WriteLine("Hello World!");

        if (false)
        {
            string connectionString = @"Data Source=10.126.242.1;Initial Catalog=Metabase;Integrated Security=False;User Id=markinni;Password=DQNd27i6avwCW5Z;";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var existingFile = new FileInfo(@"C:\Temp\DR_Final.xlsx");

            //2383 текущий максимальный id
            //2382 - всего записей

            string[] fileEntries = Directory.GetFiles("E:\\temp\\DR");

            for (int j = 0; j < fileEntries.Length; j++)
            {
                System.Console.WriteLine(fileEntries[j]);
                using var package = new ExcelPackage(fileEntries[j]);
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                using var tran = connection.BeginTransaction();


                var sheet1 = package.Workbook.Worksheets[0];
                int rowCount = sheet1.Dimension.End.Row;
                //int i = 1;
                for (int row = 2; row <= rowCount; row++)
                {
                    System.Console.WriteLine(row);
                    var product = "Образование";
                    var infsys = sheet1.Cells[row, 1].Value?.ToString();
                    var vm_name = sheet1.Cells[row, 4].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(vm_name))
                        continue;

                    if (!string.Equals("В эксплуатации", sheet1.Cells[row, 2].Value?.ToString(), StringComparison.OrdinalIgnoreCase)
                        || !string.Equals("В эксплуатации", sheet1.Cells[row, 5].Value?.ToString(), StringComparison.OrdinalIgnoreCase)
                        || !string.Equals("Продуктивная", sheet1.Cells[row, 9].Value?.ToString(), StringComparison.OrdinalIgnoreCase))
                        continue;


                    using var command0 = new SqlCommand("SELECT Count(1) FROM [dbo].[DR_VM] WHERE [Name] = @name", tran.Connection);
                    command0.Parameters.AddWithValue("name", vm_name);
                    command0.Transaction = tran;
                    var res_count = Convert.ToInt32(command0.ExecuteScalar());
                    if (res_count > 0)
                    {
                        System.Console.WriteLine(vm_name);
                        continue;
                    }

                    var infsysId = GetOrAddInfSys(tran, infsys, product);

                    using var command2 = new SqlCommand("INSERT INTO [dbo].[DR_VM] ([InfSysId],[Name],[VMType],[CPU],[RAM],[HDD],[LAN],[PTAF],[ZDK],[CriticalDescr],[Descr]) VALUES (@infsysId, @name, @vm_type, @CPU, @RAM, @HDD, @LAN, @PTAF, @ZDK, @CriticalDescr, @Descr)", tran.Connection);
                    command2.Parameters.AddWithValue("infsysId", infsysId);
                    command2.Parameters.AddWithValue("name", vm_name);
                    var vm_type = sheet1.Cells[row, 3].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(vm_type))
                        command2.Parameters.AddWithValue("vm_type", vm_type);
                    else
                        command2.Parameters.AddWithValue("vm_type", DBNull.Value);

                    command2.Parameters.AddWithValue("CPU", 0);// (sheet1.Cells[row, 5].Value as int?) ?? 0);
                    command2.Parameters.AddWithValue("RAM", 0);// (sheet1.Cells[row, 6].Value as int?) ?? 0);
                    command2.Parameters.AddWithValue("HDD", 0);// (sheet1.Cells[row, 7].Value as int?) ?? 0);
                    command2.Parameters.AddWithValue("LAN", 0);// (sheet1.Cells[row, 8].Value as int?) ?? 0);
                    command2.Parameters.AddWithValue("PTAF", string.Equals(sheet1.Cells[row, 15].Value?.ToString(), "Да", StringComparison.CurrentCultureIgnoreCase));
                    command2.Parameters.AddWithValue("ZDK", string.Equals(sheet1.Cells[row, 15].Value?.ToString(), "Да", StringComparison.CurrentCultureIgnoreCase));
                    var CriticalDescr = sheet1.Cells[row, 15].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(CriticalDescr))
                        command2.Parameters.AddWithValue("CriticalDescr", CriticalDescr);
                    else
                        command2.Parameters.AddWithValue("CriticalDescr", DBNull.Value);

                    var Descr = sheet1.Cells[row, 15].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(Descr))
                        command2.Parameters.AddWithValue("Descr", Descr);
                    else
                        command2.Parameters.AddWithValue("Descr", DBNull.Value);

                    command2.Transaction = tran;
                    var res2 = command2.ExecuteNonQuery();

                }

                tran.Commit();
            }
        }

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

        if(false)
        {
            var jopa = new Dictionary<Guid?, IList<Guid>>();
            jopa.Add(Guid.Empty, new List<Guid> { Guid.Parse("C81A8E61-FE4D-40CF-8A44-1F1EF5C1EF6A") });
            //t87654321@mail.ru
            uc.DoAddUpdateUser(new AddUser.AUser
            {
                Id = Guid.Parse("2A31E03B-1913-495C-9DC3-8C4BE81E7182"),
                FirstName = "Пользователь",
                LastName = "Пользователь",
                Password = "PdquiL@DgR5cMX",
                Email = "t87654321@mail.ru",
                Rights = jopa,
            });
        }

        if(true)
        {
            //sonarv@mail.ru
            var aid = Guid.Parse("B00FEC18-510F-498C-8268-DE60A6FD6E69");
            uc.AddOrUpdate(new DbAccount { Id = aid, Password = BCrypt.Net.BCrypt.HashString("38ktMex#3o6y6Fq!FM"), }, (source, destination) => {
                source.Password = destination.Password;
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
