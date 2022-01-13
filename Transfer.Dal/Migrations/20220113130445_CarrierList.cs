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

            migrationBuilder.DropIndex(
                name: "IX_Accounts_OrganisationId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Accounts");

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
                name: "OrganisationId",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                name: "IX_Drivers_OrganisationId",
                table: "Drivers",
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
                name: "FK_Drivers_Organisations_OrganisationId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "OrganisationAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_OrganisationId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Drivers");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OrganisationId",
                table: "Accounts",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Organisations_OrganisationId",
                table: "Accounts",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
