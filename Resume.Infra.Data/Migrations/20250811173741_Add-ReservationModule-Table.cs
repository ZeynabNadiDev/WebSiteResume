using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resume.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationModuleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationDates",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservationDateTimes",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    ReservationDateId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDateTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationDateTimes_ReservationDates_ReservationDateId",
                        column: x => x.ReservationDateId,
                        principalTable: "ReservationDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonSelectedReservations",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservationDateTimeId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSelectedReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonSelectedReservations_ReservationDateTimes_ReservationDateTimeId",
                        column: x => x.ReservationDateTimeId,
                        principalTable: "ReservationDateTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonSelectedReservations_ReservationDateTimeId",
                table: "PersonSelectedReservations",
                column: "ReservationDateTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDateTimes_ReservationDateId",
                table: "ReservationDateTimes",
                column: "ReservationDateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonSelectedReservations");

            migrationBuilder.DropTable(
                name: "ReservationDateTimes");

            migrationBuilder.DropTable(
                name: "ReservationDates");
        }
    }
}
