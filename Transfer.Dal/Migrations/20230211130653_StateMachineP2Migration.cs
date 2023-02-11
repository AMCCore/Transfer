using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class StateMachineP2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StateMachineStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StateMachine = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMachineStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateMachineActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ActionCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsSystemAction = table.Column<bool>(type: "bit", nullable: false),
                    StateMachine = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMachineActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateMachineActions_StateMachineStates_ToStateId",
                        column: x => x.ToStateId,
                        principalTable: "StateMachineStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StateMachineFromStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RightCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StateMachineActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateMachine = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMachineFromStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateMachineFromStatuses_StateMachineActions_StateMachineActionId",
                        column: x => x.StateMachineActionId,
                        principalTable: "StateMachineActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateMachineFromStatuses_StateMachineStates_FromStateId",
                        column: x => x.FromStateId,
                        principalTable: "StateMachineStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateMachineActions_ToStateId",
                table: "StateMachineActions",
                column: "ToStateId");

            migrationBuilder.CreateIndex(
                name: "IX_StateMachineFromStatuses_FromStateId",
                table: "StateMachineFromStatuses",
                column: "FromStateId");

            migrationBuilder.CreateIndex(
                name: "IX_StateMachineFromStatuses_StateMachineActionId",
                table: "StateMachineFromStatuses",
                column: "StateMachineActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateMachineFromStatuses");

            migrationBuilder.DropTable(
                name: "StateMachineActions");

            migrationBuilder.DropTable(
                name: "StateMachineStates");
        }
    }
}
