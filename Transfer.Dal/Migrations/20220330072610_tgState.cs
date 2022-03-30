using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class tgState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TgActionStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StateProps = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TgActionStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TgActionStates_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogins_LoginType_Value",
                table: "ExternalLogins",
                columns: new[] { "LoginType", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TgActionStates_AccountId",
                table: "TgActionStates",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TgActionStates");

            migrationBuilder.DropIndex(
                name: "IX_ExternalLogins_LoginType_Value",
                table: "ExternalLogins");
        }
    }
}
