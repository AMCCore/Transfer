using Microsoft.EntityFrameworkCore.Migrations;

namespace Transfer.Dal.Migrations
{
    public partial class tgStateUdp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TgActionStates",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TgActionStates");
        }
    }
}
