using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObsidianToMarkdown.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObsidianFiles",
                columns: table => new
                {
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    Sha256 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObsidianFiles", x => x.Path);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObsidianFiles");
        }
    }
}
