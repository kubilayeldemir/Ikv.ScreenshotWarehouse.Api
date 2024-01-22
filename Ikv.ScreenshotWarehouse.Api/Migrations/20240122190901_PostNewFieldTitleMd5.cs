using Microsoft.EntityFrameworkCore.Migrations;

namespace Ikv.ScreenshotWarehouse.Api.Migrations
{
    public partial class PostNewFieldTitleMd5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleMd5",
                table: "Posts",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TitleMd5",
                table: "Posts",
                column: "TitleMd5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_TitleMd5",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TitleMd5",
                table: "Posts");
        }
    }
}
