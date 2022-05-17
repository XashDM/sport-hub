using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportHub.Domain.Migrations
{
    public partial class DisplayItemsInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisplayItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DisplayLocation = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IsDisplayed = table.Column<bool>(type: "bit", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplayItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisplayItems_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisplayItems_ArticleId",
                table: "DisplayItems",
                column: "ArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisplayItems");
        }
    }
}
