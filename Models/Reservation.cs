using System.ComponentModel.DataAnnotations;

namespace AutoRentNet.Models;

public record Reservation
{
    public int Id { get; set; }

    // Foreign Key → Vehicle
    [Required]
    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; } = default!;

    // Client Data
    [Required, MaxLength(60)]
    public string CustomerName { get; set; } = default!;

    [EmailAddress]
    public string CustomerEmail { get; set; } = default!;

    // Dates and Locations
    [MaxLength(60)] public string PickupLocation  { get; set; } = default!;
    [MaxLength(60)] public string DropoffLocation { get; set; } = default!;
    public DateTime PickupDateTime  { get; set; }
    public DateTime DropoffDateTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}