using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class AccountRigthsRebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
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
                name: "OrganisationId",
                table: "AccountRights",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequestIdentifiers_TripRequestId",
                table: "TripRequestIdentifiers",
                column: "TripRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_AccountId",
                table: "AccountRights",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRights_Organisations_OrganisationId",
                table: "AccountRights");

            migrationBuilder.DropIndex(
                name: "IX_TripRequestIdentifiers_TripRequestId",
                table: "TripRequestIdentifiers");

            migrationBuilder.DropIndex(
                name: "IX_AccountRights_AccountId",
                table: "AccountRights");

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
        }
    }
}
