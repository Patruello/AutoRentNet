using AutoRentNet.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoRentNet.Data;

public class AppDbContext : DbContext
{
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Vehicle>().HasData(SeedVehicles());
        b.Entity<Reservation>().HasData(SeedReservations());
    }


    private static IEnumerable<Vehicle> SeedVehicles() =>
    [
        new Vehicle
        {
            Id = 1, Name = "BMW Seria 3", Description = "Sedan 2019", Fuel = "Benzyna", Transmission = "Automat",
            Consumption = "6.5 L/100 km", Trunk = "480 L", Doors = 4, Seats = 5, Image = "/img/fleet/bmw_3.png"
        },
        new Vehicle
        {
            Id = 2, Name = "BMW X5", Description = "SUV 2020", Fuel = "Benzyna", Transmission = "Automat",
            Consumption = "8.5 L/100 km", Trunk = "650 L", Doors = 5, Seats = 5, Image = "/img/fleet/bmw_x5.jpg"
        },
        new Vehicle
        {
            Id = 3, Name = "Citroen C3", Description = "Hatchback 2018", Fuel = "Diesel", Transmission = "Manual",
            Consumption = "4.5 L/100 km", Trunk = "300 L", Doors = 5, Seats = 5, Image = "/img/fleet/citroen_c3.png"
        },
        new Vehicle
        {
            Id = 4, Name = "Fiat 500", Description = "City Car 2019", Fuel = "Benzyna", Transmission = "Automat",
            Consumption = "5 L/100 km", Trunk = "185 L", Doors = 3, Seats = 5, Image = "/img/fleet/fiat_500.png"
        },
        new Vehicle
        {
            Id = 5, Name = "Honda CRV", Description = "SUV 2021", Fuel = "Hybryda", Transmission = "Automat",
            Consumption = "6 L/100 km", Trunk = "550 L", Doors = 5, Seats = 5, Image = "/img/fleet/honda_crv.jpg"
        },
        new Vehicle
        {
            Id = 6, Name = "Seat Leon", Description = "Hatchback 2022", Fuel = "Diesel", Transmission = "Manual",
            Consumption = "4.8 L/100 km", Trunk = "380 L", Doors = 5, Seats = 5, Image = "/img/fleet/seat_leon.png"
        },
        new Vehicle
        {
            Id = 7, Name = "Skoda Superb", Description = "SUV 2022", Fuel = "Diesel", Transmission = "Automat",
            Consumption = "5.2 L/100 km", Trunk = "6250 L", Doors = 4, Seats = 5, Image = "/img/fleet/skoda_superb.png"
        },
        new Vehicle
        {
            Id = 8, Name = "Toyota Yaris", Description = "Hatchback 2023", Fuel = "Hybryda", Transmission = "Automat",
            Consumption = "3.8 L/100 km", Trunk = "286 L", Doors = 5, Seats = 5, Image = "/img/fleet/toyota_yaris.png"
        },
        new Vehicle
        {
            Id = 9, Name = "Toyota C-HR", Description = "Crossover 2024", Fuel = "Hybryda", Transmission = "Automat",
            Consumption = "4.1 L/100 km", Trunk = "377 L", Doors = 5, Seats = 5, Image = "/img/fleet/toyota-c-hr.jpg"
        },
        new Vehicle
        {
            Id = 10, Name = "BMW Seria 1", Description = "Hatchback 2019", Fuel = "Diesel", Transmission = "Automat",
            Consumption = "5.5 L/100 km", Trunk = "360 L", Doors = 5, Seats = 5, Image = "/img/fleet/bmw_1.png"
        }
    ];
    
    private static IEnumerable<Reservation> SeedReservations() =>
    [
        new Reservation { Id = 1, VehicleId = 1,
            CustomerName = "Jan Kowalski", CustomerEmail = "jan@example.com",
            PickupLocation = "Lotnisko Lublin", DropoffLocation = "Lotnisko Lublin",
            PickupDateTime = new DateTime(2025, 7, 1), DropoffDateTime = new DateTime(2025, 7, 4),
            CreatedAt = new DateTime(2025, 6, 1, 17, 30, 0, DateTimeKind.Utc) },
        new Reservation { Id = 2, VehicleId = 2,
            CustomerName = "Anna Nowak", CustomerEmail = "anna@example.com",
            PickupLocation = "Lotnisko Lublin", DropoffLocation = "Lotnisko Lublin",
            PickupDateTime = new DateTime(2025, 8, 16, 9, 0, 0, DateTimeKind.Utc), DropoffDateTime = new DateTime(2025, 8, 20, 18, 0, 0, DateTimeKind.Utc),
            CreatedAt = new DateTime(2025, 6, 3, 12, 0, 0, DateTimeKind.Utc) }
    ];
}