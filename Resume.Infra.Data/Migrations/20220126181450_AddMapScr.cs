using Microsoft.EntityFrameworkCore.Migrations;

namespace Resume.Infra.Data.Migrations
{
    public partial class AddMapScr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MapSrc",
                table: "Information",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapSrc",
                table: "Information");
        }
    }
}
