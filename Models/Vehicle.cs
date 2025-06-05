using System.ComponentModel.DataAnnotations;

namespace AutoRentNet.Models;

public record Vehicle
{
    public int Id { get; init; }
    
    [Required, MaxLength(60)]
    public string Name { get; init; } = default!;

    public string? Description { get; init; }

    [MaxLength(20)]
    public string? Fuel { get; init; }

    [MaxLength(20)]
    public string? Transmission { get; init; }
    
    public string? Consumption { get; init; }

    public string? Trunk { get; init; }

    public int Doors { get; init; }

    public int Seats { get; init; }

    // Relatywna ścieżka do obrazu w wwwroot/img
    public string? Image { get; init; }

    // Nawigacja → lista rezerwacji tego pojazdu (EF Core)
    public ICollection<Reservation>? Reservations { get; init; }
}