using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportHub.Domain.Migrations
{
    public partial class AddedDisplayedLanguages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DSLanguageName",
                table: "DisplayedLanguages",
                newName: "LanguageName");

            migrationBuilder.RenameColumn(
                name: "DSIsEnabled",
                table: "DisplayedLanguages",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "DSId",
                table: "DisplayedLanguages",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LanguageName",
                table: "DisplayedLanguages",
                newName: "DSLanguageName");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "DisplayedLanguages",
                newName: "DSIsEnabled");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DisplayedLanguages",
                newName: "DSId");
        }
    }
}
