using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class IndexChangeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountRights_Rights_RightId",
                table: "AccountRights");

            migrationBuilder.DropIndex(
                name: "IX_TripRequestIdentifiers_Identifier",
                table: "TripRequestIdentifiers");

            migrationBuilder.DropIndex(
                name: "IX_TripRequestIdentifiers_TripRequestId",
                table: "TripRequestIdentifiers");

            migrationBuilder.DropIndex(
                name: "IX_ExternalLogins_LoginType_Value",
                table: "ExternalLogins");

            migrationBuilder.DropIndex(
                name: "IX_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights");

            migrationBuilder.AlterColumn<Guid>(
                name: "RightId",
                table: "AccountRights",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "AccountRights",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TripRequestIdentifiers_Identifier",
                table: "TripRequestIdentifiers",
                column: "Identifier");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TripRequestIdentifiers_TripRequestId",
                table: "TripRequestIdentifiers",
                column: "TripRequestId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ExternalLogins_LoginType_Value",
                table: "ExternalLogins",
                columns: new[] { "LoginType", "Value" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights",
                columns: new[] { "AccountId", "RightId", "OrganisationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRights_Rights_RightId",
                table: "AccountRights",
                column: "RightId",
                principalTable: "Rights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountRights_Rights_RightId",
                table: "AccountRights");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TripRequestIdentifiers_Identifier",
                table: "TripRequestIdentifiers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TripRequestIdentifiers_TripRequestId",
                table: "TripRequestIdentifiers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ExternalLogins_LoginType_Value",
                table: "ExternalLogins");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights");

            migrationBuilder.AlterColumn<Guid>(
                name: "RightId",
                table: "AccountRights",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "AccountRights",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequestIdentifiers_Identifier",
                table: "TripRequestIdentifiers",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripRequestIdentifiers_TripRequestId",
                table: "TripRequestIdentifiers",
                column: "TripRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogins_LoginType_Value",
                table: "ExternalLogins",
                columns: new[] { "LoginType", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_AccountId_RightId_OrganisationId",
                table: "AccountRights",
                columns: new[] { "AccountId", "RightId", "OrganisationId" },
                unique: true,
                filter: "[RightId] IS NOT NULL AND [OrganisationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRights_Rights_RightId",
                table: "AccountRights",
                column: "RightId",
                principalTable: "Rights",
                principalColumn: "Id");
        }
    }
}
