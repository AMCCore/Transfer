using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class CarrierList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Organisations_OrganisationId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_PersonDatas_PersonDataId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_DriverLicenses_DbDriversLicenseId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_PersonDatas_PersonDataId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "PersonDatas");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_DbDriversLicenseId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_OrganisationId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_PersonDataId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights");

            migrationBuilder.DropColumn(
                name: "DbDriversLicenseId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PersonDataId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "PersonDataId",
                table: "Drivers",
                newName: "OrganisationId");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_PersonDataId",
                table: "Drivers",
                newName: "IX_Drivers_OrganisationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                table: "Organisations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "Organisations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Organisations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "DriverLicenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "AccountRights",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OrganisationAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    AccountType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganisationAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganisationAccounts_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverLicenses_DriverId",
                table: "DriverLicenses",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights",
                columns: new[] { "AccountId", "RightId", "OrganisationId" },
                unique: true,
                filter: "[RightId] IS NOT NULL AND [OrganisationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_OrganisationId",
                table: "AccountRights",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationAccounts_AccountId",
                table: "OrganisationAccounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationAccounts_OrganisationId",
                table: "OrganisationAccounts",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverLicenses_Drivers_DriverId",
                table: "DriverLicenses",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Organisations_OrganisationId",
                table: "Drivers",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverLicenses_Drivers_DriverId",
                table: "DriverLicenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Organisations_OrganisationId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "OrganisationAccounts");

            migrationBuilder.DropIndex(
                name: "IX_DriverLicenses_DriverId",
                table: "DriverLicenses");

            migrationBuilder.DropIndex(
                name: "IX_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights");

            migrationBuilder.DropIndex(
                name: "IX_AccountRights_OrganisationId",
                table: "AccountRights");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "DriverLicenses");

            migrationBuilder.RenameColumn(
                name: "OrganisationId",
                table: "Drivers",
                newName: "PersonDataId");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_OrganisationId",
                table: "Drivers",
                newName: "IX_Drivers_PersonDataId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                table: "Organisations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DbDriversLicenseId",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PersonDataId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "AccountRights",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "PersonDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentDateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentIssurer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentSeries = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DocumentSubDivisionCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsMale = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RealAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RegistrationAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_DbDriversLicenseId",
                table: "Drivers",
                column: "DbDriversLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OrganisationId",
                table: "Accounts",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PersonDataId",
                table: "Accounts",
                column: "PersonDataId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights",
                columns: new[] { "AccountId", "RightId", "OrganisationId" },
                unique: true,
                filter: "[AccountId] IS NOT NULL AND [RightId] IS NOT NULL AND [OrganisationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Organisations_OrganisationId",
                table: "Accounts",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_PersonDatas_PersonDataId",
                table: "Accounts",
                column: "PersonDataId",
                principalTable: "PersonDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_DriverLicenses_DbDriversLicenseId",
                table: "Drivers",
                column: "DbDriversLicenseId",
                principalTable: "DriverLicenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_PersonDatas_PersonDataId",
                table: "Drivers",
                column: "PersonDataId",
                principalTable: "PersonDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
