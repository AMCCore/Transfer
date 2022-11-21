using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class AddDateCreationAndCreatorMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Organisations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Organisations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Drivers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Buses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Buses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_CreatorId",
                table: "Organisations",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CreatorId",
                table: "Drivers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_CreatorId",
                table: "Buses",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Accounts_CreatorId",
                table: "Buses",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Accounts_CreatorId",
                table: "Drivers",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisations_Accounts_CreatorId",
                table: "Organisations",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Accounts_CreatorId",
                table: "Buses");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Accounts_CreatorId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Organisations_Accounts_CreatorId",
                table: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Organisations_CreatorId",
                table: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CreatorId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Buses_CreatorId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Buses");
        }
    }
}
