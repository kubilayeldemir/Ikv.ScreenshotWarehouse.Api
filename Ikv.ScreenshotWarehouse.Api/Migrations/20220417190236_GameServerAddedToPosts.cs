using Microsoft.EntityFrameworkCore.Migrations;

namespace Ikv.ScreenshotWarehouse.Api.Migrations
{
    public partial class GameServerAddedToPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "GameServer",
                table: "Posts",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameServer",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Posts",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);
        }
    }
}
