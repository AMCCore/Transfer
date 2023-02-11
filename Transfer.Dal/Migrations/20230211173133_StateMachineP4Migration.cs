using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class StateMachineP4Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActionState",
                table: "TripRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("950C380C-55E2-4D97-82A7-CCF3F8A87D72"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionState",
                table: "TripRequests");
        }
    }
}
