using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class Files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Extention = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganisationFiles_Accounts_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganisationFiles_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationFiles_OrganisationId",
                table: "OrganisationFiles",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationFiles_UploaderId",
                table: "OrganisationFiles",
                column: "UploaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "OrganisationFiles");
        }
    }
}
