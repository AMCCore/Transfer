using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class todo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripRequestIdentifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    Identifier = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "15426, 1"),
                    TripRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripRequestIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripRequestIdentifiers_TripRequests_TripRequestId",
                        column: x => x.TripRequestId,
                        principalTable: "TripRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripRequestIdentifiers");
        }
    }
}
