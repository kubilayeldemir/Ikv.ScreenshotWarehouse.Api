using Microsoft.EntityFrameworkCore.Migrations;

namespace Ikv.ScreenshotWarehouse.Api.Migrations
{
    public partial class NewIndexesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_IsValidated",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TitleMd5",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Category_CreatedAt",
                table: "Posts",
                columns: new[] { "Category", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Category_ScreenshotDate",
                table: "Posts",
                columns: new[] { "Category", "ScreenshotDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TitleMd5_ScreenshotDate",
                table: "Posts",
                columns: new[] { "TitleMd5", "ScreenshotDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Username_ScreenshotDate",
                table: "Posts",
                columns: new[] { "Username", "ScreenshotDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_Category_CreatedAt",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Category_ScreenshotDate",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TitleMd5_ScreenshotDate",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Username_ScreenshotDate",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsValidated",
                table: "Posts",
                column: "IsValidated");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TitleMd5",
                table: "Posts",
                column: "TitleMd5");
        }
    }
}
