using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class MachineStateV2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateValid",
                table: "TripRequestReplays");

            migrationBuilder.AddColumn<bool>(
                name: "UseSystem",
                table: "StateMachineStates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseSystem",
                table: "StateMachineStates");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateValid",
                table: "TripRequestReplays",
                type: "datetime2",
                nullable: true);
        }
    }
}
