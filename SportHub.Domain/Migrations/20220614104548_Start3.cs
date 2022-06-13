using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportHub.Domain.Migrations
{
    public partial class Start3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlternativeTextForThePicture",
                table: "Articles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlternativeTextForThePicture",
                table: "Articles");
        }
    }
}
