using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverLicenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    DocumentSeries = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentDateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentEndDateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentIssurer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentCatigories = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLicenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsMale = table.Column<bool>(type: "bit", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DocumentSeries = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentSubDivisionCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentIssurer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentDateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RealAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PersonDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DbDriversLicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TelegramId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_DriverLicenses_DbDriversLicenseId",
                        column: x => x.DbDriversLicenseId,
                        principalTable: "DriverLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drivers_PersonDatas_PersonDataId",
                        column: x => x.PersonDataId,
                        principalTable: "PersonDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    INN = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OGRN = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DirectorFio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DirectorPosition = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organisations_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PersonDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_PersonDatas_PersonDataId",
                        column: x => x.PersonDataId,
                        principalTable: "PersonDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationWorkingAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationWorkingAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganisationWorkingAreas_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganisationWorkingAreas_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountRights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    RightId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRights_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountRights_Rights_RightId",
                        column: x => x.RightId,
                        principalTable: "Rights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SubValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalLogins_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights",
                columns: new[] { "AccountId", "RightId", "OrganisationId" },
                unique: true,
                filter: "[AccountId] IS NOT NULL AND [RightId] IS NOT NULL AND [OrganisationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_RightId",
                table: "AccountRights",
                column: "RightId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OrganisationId",
                table: "Accounts",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PersonDataId",
                table: "Accounts",
                column: "PersonDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_DbDriversLicenseId",
                table: "Drivers",
                column: "DbDriversLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_PersonDataId",
                table: "Drivers",
                column: "PersonDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogins_AccountId",
                table: "ExternalLogins",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_RegionId",
                table: "Organisations",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationWorkingAreas_OrganisationId",
                table: "OrganisationWorkingAreas",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationWorkingAreas_RegionId",
                table: "OrganisationWorkingAreas",
                column: "RegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRights");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "ExternalLogins");

            migrationBuilder.DropTable(
                name: "OrganisationWorkingAreas");

            migrationBuilder.DropTable(
                name: "Rights");

            migrationBuilder.DropTable(
                name: "DriverLicenses");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropTable(
                name: "PersonDatas");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
