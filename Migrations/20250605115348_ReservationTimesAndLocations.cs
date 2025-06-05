using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoRentNet.Migrations
{
    /// <inheritdoc />
    public partial class ReservationTimesAndLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Reservations",
                newName: "PickupDateTime");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Reservations",
                newName: "DropoffDateTime");

            migrationBuilder.AddColumn<string>(
                name: "DropoffLocation",
                table: "Reservations",
                type: "TEXT",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PickupLocation",
                table: "Reservations",
                type: "TEXT",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DropoffLocation", "PickupLocation" },
                values: new object[] { "Lotnisko Lublin", "Lotnisko Lublin" });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DropoffDateTime", "DropoffLocation", "PickupDateTime", "PickupLocation" },
                values: new object[] { new DateTime(2025, 8, 20, 18, 0, 0, 0, DateTimeKind.Utc), "Lotnisko Lublin", new DateTime(2025, 8, 16, 9, 0, 0, 0, DateTimeKind.Utc), "Lotnisko Lublin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropoffLocation",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PickupLocation",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "PickupDateTime",
                table: "Reservations",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "DropoffDateTime",
                table: "Reservations",
                newName: "EndDate");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
