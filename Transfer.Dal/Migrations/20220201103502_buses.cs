using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class buses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "OrganisationFiles",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "EMail",
                table: "Drivers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Drivers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Drivers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Drivers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Drivers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Buses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Make = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Yaer = table.Column<int>(type: "int", nullable: false),
                    PeopleCopacity = table.Column<int>(type: "int", nullable: false),
                    LuggageVolume = table.Column<int>(type: "int", nullable: true),
                    TV = table.Column<bool>(type: "bit", nullable: false),
                    AirConditioner = table.Column<bool>(type: "bit", nullable: false),
                    SaftyBelts = table.Column<bool>(type: "bit", nullable: false),
                    Audio = table.Column<bool>(type: "bit", nullable: false),
                    WC = table.Column<bool>(type: "bit", nullable: false),
                    Microphone = table.Column<bool>(type: "bit", nullable: false),
                    Wifi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buses_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverFiles_Accounts_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverFiles_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusFiles_Accounts_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusFiles_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationFiles_FileId",
                table: "OrganisationFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_OrganisationId",
                table: "Buses",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_BusFiles_BusId",
                table: "BusFiles",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusFiles_FileId",
                table: "BusFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_BusFiles_UploaderId",
                table: "BusFiles",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverFiles_DriverId",
                table: "DriverFiles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverFiles_FileId",
                table: "DriverFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverFiles_UploaderId",
                table: "DriverFiles",
                column: "UploaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationFiles_Files_FileId",
                table: "OrganisationFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationFiles_Files_FileId",
                table: "OrganisationFiles");

            migrationBuilder.DropTable(
                name: "BusFiles");

            migrationBuilder.DropTable(
                name: "DriverFiles");

            migrationBuilder.DropTable(
                name: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_OrganisationFiles_FileId",
                table: "OrganisationFiles");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "OrganisationFiles");

            migrationBuilder.DropColumn(
                name: "EMail",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Drivers");
        }
    }
}
