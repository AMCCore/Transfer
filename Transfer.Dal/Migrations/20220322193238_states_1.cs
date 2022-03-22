using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class states_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "State",
                table: "TripRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("D18D3114-582F-4E30-A1E2-8E84A9F7F31B"));

            migrationBuilder.AddColumn<Guid>(
                name: "State",
                table: "Organisations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("9AEC0B81-230D-4137-B4DD-A1B51B5EB467"));

            migrationBuilder.AddColumn<Guid>(
                name: "State",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("5EC8CDBD-9681-4E4B-BE96-541E0C4F4703"));

            migrationBuilder.AddColumn<Guid>(
                name: "State",
                table: "Buses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("24554358-C796-4355-A9FD-34052A66A8CD"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Buses");
        }
    }
}
