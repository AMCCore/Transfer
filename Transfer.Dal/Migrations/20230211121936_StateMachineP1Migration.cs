using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class StateMachineP1Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateMachineStateRights");

            migrationBuilder.DropTable(
                name: "StateMachineStates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StateMachineStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ButtonName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ConfirmText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    StateFrom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateMachine = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateTo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UseByAuthorized = table.Column<bool>(type: "bit", nullable: false),
                    UseByOrganisation = table.Column<bool>(type: "bit", nullable: false),
                    UseByOwner = table.Column<bool>(type: "bit", nullable: false),
                    UseBySystem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMachineStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateMachineStateRights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RightId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateMachineStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMachineStateRights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateMachineStateRights_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateMachineStateRights_Rights_RightId",
                        column: x => x.RightId,
                        principalTable: "Rights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateMachineStateRights_StateMachineStates_StateMachineStateId",
                        column: x => x.StateMachineStateId,
                        principalTable: "StateMachineStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateMachineStateRights_OrganisationId",
                table: "StateMachineStateRights",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_StateMachineStateRights_RightId",
                table: "StateMachineStateRights",
                column: "RightId");

            migrationBuilder.CreateIndex(
                name: "IX_StateMachineStateRights_StateMachineStateId_RightId",
                table: "StateMachineStateRights",
                columns: new[] { "StateMachineStateId", "RightId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StateMachineStates_StateFrom_StateTo_UseByOrganisation",
                table: "StateMachineStates",
                columns: new[] { "StateFrom", "StateTo", "UseByOrganisation" },
                unique: true);
        }
    }
}
