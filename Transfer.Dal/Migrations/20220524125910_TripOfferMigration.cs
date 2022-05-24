using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class TripOfferMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TripRequestReplays");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateValid",
                table: "TripRequestReplays",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TripRequestOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TripRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripRequestOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripRequestOffers_Organisations_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripRequestOffers_TripRequests_TripRequestId",
                        column: x => x.TripRequestId,
                        principalTable: "TripRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripRequestOffers_CarrierId",
                table: "TripRequestOffers",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequestOffers_TripRequestId",
                table: "TripRequestOffers",
                column: "TripRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripRequestOffers");

            migrationBuilder.DropColumn(
                name: "DateValid",
                table: "TripRequestReplays");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TripRequestReplays",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
