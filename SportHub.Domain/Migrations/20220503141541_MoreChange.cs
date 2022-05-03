using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportHub.Domain.Migrations
{
    public partial class MoreChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_NavigationItems_ItemId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_NavigationItems_NavigationItems_FatherItemId",
                table: "NavigationItems");

            migrationBuilder.RenameColumn(
                name: "FatherItemId",
                table: "NavigationItems",
                newName: "ParentsItemId");

            migrationBuilder.RenameIndex(
                name: "IX_NavigationItems_FatherItemId",
                table: "NavigationItems",
                newName: "IX_NavigationItems_ParentsItemId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Articles",
                newName: "ReferenceItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_ItemId",
                table: "Articles",
                newName: "IX_Articles_ReferenceItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_NavigationItems_ReferenceItemId",
                table: "Articles",
                column: "ReferenceItemId",
                principalTable: "NavigationItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationItems_NavigationItems_ParentsItemId",
                table: "NavigationItems",
                column: "ParentsItemId",
                principalTable: "NavigationItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_NavigationItems_ReferenceItemId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_NavigationItems_NavigationItems_ParentsItemId",
                table: "NavigationItems");

            migrationBuilder.RenameColumn(
                name: "ParentsItemId",
                table: "NavigationItems",
                newName: "FatherItemId");

            migrationBuilder.RenameIndex(
                name: "IX_NavigationItems_ParentsItemId",
                table: "NavigationItems",
                newName: "IX_NavigationItems_FatherItemId");

            migrationBuilder.RenameColumn(
                name: "ReferenceItemId",
                table: "Articles",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_ReferenceItemId",
                table: "Articles",
                newName: "IX_Articles_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_NavigationItems_ItemId",
                table: "Articles",
                column: "ItemId",
                principalTable: "NavigationItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationItems_NavigationItems_FatherItemId",
                table: "NavigationItems",
                column: "FatherItemId",
                principalTable: "NavigationItems",
                principalColumn: "Id");
        }
    }
}
