using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class TripRequestRegions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegionFromId",
                table: "TripRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegionToId",
                table: "TripRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_RegionFromId",
                table: "TripRequests",
                column: "RegionFromId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_RegionToId",
                table: "TripRequests",
                column: "RegionToId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_Regions_RegionFromId",
                table: "TripRequests",
                column: "RegionFromId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_Regions_RegionToId",
                table: "TripRequests",
                column: "RegionToId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_Regions_RegionFromId",
                table: "TripRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_Regions_RegionToId",
                table: "TripRequests");

            migrationBuilder.DropIndex(
                name: "IX_TripRequests_RegionFromId",
                table: "TripRequests");

            migrationBuilder.DropIndex(
                name: "IX_TripRequests_RegionToId",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "RegionFromId",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "RegionToId",
                table: "TripRequests");
        }
    }
}
