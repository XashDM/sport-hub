using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportHub.Domain.Migrations
{
    public partial class ImageItemToDisplayItemAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageItemId",
                table: "DisplayItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisplayItems_ImageItemId",
                table: "DisplayItems",
                column: "ImageItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisplayItems_ImageItems_ImageItemId",
                table: "DisplayItems",
                column: "ImageItemId",
                principalTable: "ImageItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisplayItems_ImageItems_ImageItemId",
                table: "DisplayItems");

            migrationBuilder.DropIndex(
                name: "IX_DisplayItems_ImageItemId",
                table: "DisplayItems");

            migrationBuilder.DropColumn(
                name: "ImageItemId",
                table: "DisplayItems");
        }
    }
}
