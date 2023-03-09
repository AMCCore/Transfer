using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transfer.Dal.Migrations
{
    public partial class StateMachineMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateMachineStateRights");

            migrationBuilder.DropIndex(
                name: "IX_StateMachineStates_StateFrom_StateTo_UseByOrganisation",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "ButtonName",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "ConfirmText",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "StateFrom",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "StateTo",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "UseByAuthorized",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "UseByOrganisation",
                table: "StateMachineStates");

            migrationBuilder.DropColumn(
                name: "UseByOwner",
                table: "StateMachineStates");

            migrationBuilder.RenameColumn(
                name: "UseBySystem",
                table: "StateMachineStates",
                newName: "IsDeleted");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StateMachineStates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StateMachineStates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StateMachineActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdateTick = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ConfirmText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ActionCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsSystemAction = table.Column<bool>(type: "bit", nullable: false),
                    StateMachine = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortingOrder = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StateMachineStates");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "StateMachineStates",
                newName: "UseBySystem");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StateMachineStates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonName",
                table: "StateMachineStates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmText",
                table: "StateMachineStates",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StateMachineStates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "StateFrom",
                table: "StateMachineStates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StateTo",
                table: "StateMachineStates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "UseByAuthorized",
                table: "StateMachineStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseByOrganisation",
                table: "StateMachineStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseByOwner",
                table: "StateMachineStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "IX_StateMachineStates_StateFrom_StateTo_UseByOrganisation",
                table: "StateMachineStates",
                columns: new[] { "StateFrom", "StateTo", "UseByOrganisation" },
                unique: true);

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
        }
    }
}
