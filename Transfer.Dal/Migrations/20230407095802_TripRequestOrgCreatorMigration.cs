using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class TripRequestOrgCreatorMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrgCreatorId",
                table: "TripRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_OrgCreatorId",
                table: "TripRequests",
                column: "OrgCreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_Organisations_OrgCreatorId",
                table: "TripRequests",
                column: "OrgCreatorId",
                principalTable: "Organisations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_Organisations_OrgCreatorId",
                table: "TripRequests");

            migrationBuilder.DropIndex(
                name: "IX_TripRequests_OrgCreatorId",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "OrgCreatorId",
                table: "TripRequests");
        }
    }
}
