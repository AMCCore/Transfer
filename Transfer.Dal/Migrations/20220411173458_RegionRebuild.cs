using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class RegionRebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisations_Regions_RegionId",
                table: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Organisations_RegionId",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Organisations");

            migrationBuilder.AddColumn<Guid>(
                name: "ParrentRegionId",
                table: "Regions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParrentRegionId",
                table: "Regions",
                column: "ParrentRegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Regions_ParrentRegionId",
                table: "Regions",
                column: "ParrentRegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Regions_ParrentRegionId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_ParrentRegionId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ParrentRegionId",
                table: "Regions");

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                table: "Organisations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_RegionId",
                table: "Organisations",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisations_Regions_RegionId",
                table: "Organisations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }
    }
}
