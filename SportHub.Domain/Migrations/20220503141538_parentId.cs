using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportHub.Domain.Migrations
{
    public partial class parentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationItems_NavigationItems_FatherItemId",
                table: "NavigationItems");

            migrationBuilder.RenameColumn(
                name: "FatherItemId",
                table: "NavigationItems",
                newName: "ParentItemId");

            migrationBuilder.RenameIndex(
                name: "IX_NavigationItems_FatherItemId",
                table: "NavigationItems",
                newName: "IX_NavigationItems_ParentItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationItems_NavigationItems_ParentItemId",
                table: "NavigationItems",
                column: "ParentItemId",
                principalTable: "NavigationItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationItems_NavigationItems_ParentItemId",
                table: "NavigationItems");

            migrationBuilder.RenameColumn(
                name: "ParentItemId",
                table: "NavigationItems",
                newName: "FatherItemId");

            migrationBuilder.RenameIndex(
                name: "IX_NavigationItems_ParentItemId",
                table: "NavigationItems",
                newName: "IX_NavigationItems_FatherItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationItems_NavigationItems_FatherItemId",
                table: "NavigationItems",
                column: "FatherItemId",
                principalTable: "NavigationItems",
                principalColumn: "Id");
        }
    }
}
