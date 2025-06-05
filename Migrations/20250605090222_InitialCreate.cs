using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AutoRentNet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Fuel = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Transmission = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Consumption = table.Column<string>(type: "TEXT", nullable: true),
                    Trunk = table.Column<string>(type: "TEXT", nullable: true),
                    Doors = table.Column<int>(type: "INTEGER", nullable: false),
                    Seats = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    CustomerEmail = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Consumption", "Description", "Doors", "Fuel", "Image", "Name", "Seats", "Transmission", "Trunk" },
                values: new object[,]
                {
                    { 1, "6.5 L/100 km", "Sedan 2019", 4, "Benzyna", "/img/fleet/bmw_3.png", "BMW Seria 3", 5, "Automat", "480 L" },
                    { 2, "8.5 L/100 km", "SUV 2020", 5, "Benzyna", "/img/fleet/bmw_x5.jpg", "BMW X5", 5, "Automat", "650 L" },
                    { 3, "4.5 L/100 km", "Hatchback 2018", 5, "Diesel", "/img/fleet/citroen_c3.png", "Citroen C3", 5, "Manual", "300 L" },
                    { 4, "5 L/100 km", "City Car 2019", 3, "Benzyna", "/img/fleet/fiat_500.png", "Fiat 500", 5, "Automat", "185 L" },
                    { 5, "6 L/100 km", "SUV 2021", 5, "Hybryda", "/img/fleet/honda_crv.jpg", "Honda CRV", 5, "Automat", "550 L" },
                    { 6, "4.8 L/100 km", "Hatchback 2022", 5, "Diesel", "/img/fleet/seat_leon.png", "Seat Leon", 5, "Manual", "380 L" },
                    { 7, "5.2 L/100 km", "SUV 2022", 4, "Diesel", "/img/fleet/skoda_superb.png", "Skoda Superb", 5, "Automat", "6250 L" },
                    { 8, "3.8 L/100 km", "Hatchback 2023", 5, "Hybryda", "/img/fleet/toyota_yaris.png", "Toyota Yaris", 5, "Automat", "286 L" },
                    { 9, "4.1 L/100 km", "Crossover 2024", 5, "Hybryda", "/img/fleet/toyota-c-hr.jpg", "Toyota C-HR", 5, "Automat", "377 L" },
                    { 10, "5.5 L/100 km", "Hatchback 2019", 5, "Diesel", "/img/fleet/bmw_1.png", "BMW Seria 1", 5, "Automat", "360 L" }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CreatedAt", "CustomerEmail", "CustomerName", "EndDate", "StartDate", "VehicleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 1, 17, 30, 0, 0, DateTimeKind.Utc), "jan@example.com", "Jan Kowalski", new DateTime(2025, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2025, 6, 3, 12, 0, 0, 0, DateTimeKind.Utc), "anna@example.com", "Anna Nowak", new DateTime(2025, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VehicleId",
                table: "Reservations",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
