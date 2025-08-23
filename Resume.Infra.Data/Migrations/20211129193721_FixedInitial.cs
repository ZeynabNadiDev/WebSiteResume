using Microsoft.EntityFrameworkCore.Migrations;

namespace Resume.Infra.Data.Migrations
{
    public partial class FixedInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desctiption",
                table: "thingIDos",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "thingIDos",
                newName: "Desctiption");
        }
    }
}
