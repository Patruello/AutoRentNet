using System.ComponentModel.DataAnnotations;

namespace AutoRentNet.Models;

public record Reservation
{
    public int Id { get; set; }

    // Klucz obcy → Vehicle
    [Required]
    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; } = default!;

    // Dane klienta
    [Required, MaxLength(60)]
    public string CustomerName { get; set; } = default!;

    [EmailAddress]
    public string CustomerEmail { get; set; } = default!;

    // Daty rezerwacji
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}