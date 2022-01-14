using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class DriverPersonData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonDatas_Accounts_AccountId",
                table: "PersonDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonDatas_Drivers_DriverId",
                table: "PersonDatas");

            migrationBuilder.DropIndex(
                name: "IX_PersonDatas_AccountId",
                table: "PersonDatas");

            migrationBuilder.DropIndex(
                name: "IX_PersonDatas_DriverId",
                table: "PersonDatas");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "PersonDatas");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "PersonDatas");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonDataId",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DriverLicenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PersonDataId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_PersonDataId",
                table: "Drivers",
                column: "PersonDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PersonDataId",
                table: "Accounts",
                column: "PersonDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_PersonDatas_PersonDataId",
                table: "Accounts",
                column: "PersonDataId",
                principalTable: "PersonDatas",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_PersonDatas_PersonDataId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_PersonDatas_PersonDataId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_PersonDataId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_PersonDataId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PersonDataId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DriverLicenses");

            migrationBuilder.DropColumn(
                name: "PersonDataId",
                table: "Accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "PersonDatas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "PersonDatas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonDatas_AccountId",
                table: "PersonDatas",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDatas_DriverId",
                table: "PersonDatas",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonDatas_Accounts_AccountId",
                table: "PersonDatas",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonDatas_Drivers_DriverId",
                table: "PersonDatas",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
