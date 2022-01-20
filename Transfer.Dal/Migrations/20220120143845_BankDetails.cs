using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class BankDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Agreement",
                table: "Organisations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Organisations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FactAddress",
                table: "Organisations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Organisations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BankDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Bik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Inn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kpp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KorAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAccount = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDetails_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_OrganisationId",
                table: "BankDetails",
                column: "OrganisationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDetails");

            migrationBuilder.DropColumn(
                name: "Agreement",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "FactAddress",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Organisations");
        }
    }
}
