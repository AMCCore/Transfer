using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class TripRequest_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_Organisations_СhartererId",
                table: "TripRequests");

            migrationBuilder.RenameColumn(
                name: "СhartererId",
                table: "TripRequests",
                newName: "ChartererId");

            migrationBuilder.RenameIndex(
                name: "IX_TripRequests_СhartererId",
                table: "TripRequests",
                newName: "IX_TripRequests_ChartererId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_Organisations_ChartererId",
                table: "TripRequests",
                column: "ChartererId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_Organisations_ChartererId",
                table: "TripRequests");

            migrationBuilder.RenameColumn(
                name: "ChartererId",
                table: "TripRequests",
                newName: "СhartererId");

            migrationBuilder.RenameIndex(
                name: "IX_TripRequests_ChartererId",
                table: "TripRequests",
                newName: "IX_TripRequests_СhartererId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_Organisations_СhartererId",
                table: "TripRequests",
                column: "СhartererId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
