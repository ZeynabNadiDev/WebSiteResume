using Microsoft.EntityFrameworkCore.Migrations;

namespace Resume.Infra.Data.Migrations
{
    public partial class AddOrderToThingIDoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "thingIDos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "thingIDos");
        }
    }
}
