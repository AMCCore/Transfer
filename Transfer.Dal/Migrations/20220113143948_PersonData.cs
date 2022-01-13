using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class PersonData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    RealAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonDatas_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonDatas_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonDatas_AccountId",
                table: "PersonDatas",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDatas_DriverId",
                table: "PersonDatas",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonDatas");
        }
    }
}
