using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Transfer.Common.Enums.States;
using Transfer.Common.Extensions;

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
                defaultValue: TripRequestStateEnum.New.GetEnumGuid());

            migrationBuilder.AddColumn<Guid>(
                name: "State",
                table: "Organisations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: OrganisationStateEnum.Checked.GetEnumGuid());

            migrationBuilder.AddColumn<Guid>(
                name: "State",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: DriverStateEnum.Checked.GetEnumGuid());

            migrationBuilder.AddColumn<Guid>(
                name: "State",
                table: "Buses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: BusStateEnum.Checked.GetEnumGuid());
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
