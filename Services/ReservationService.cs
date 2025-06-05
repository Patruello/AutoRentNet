using AutoRentNet.Data;
using AutoRentNet.Dtos;
using AutoRentNet.Models;
using Microsoft.EntityFrameworkCore;

public static class ReservationLogic
{
    public static async Task<(bool ok, string? error, Reservation? entity)>
        CreateAsync(ReservationDto dto, AppDbContext db)
    {
        // 1. Walidacja zakresu czasu
        if (dto.PickupDateTime >= dto.DropoffDateTime)
            return (false, "Pickup must be earlier than drop-off", null);

        // 2. Kolizja z inną rezerwacją tego samego auta
        bool overlap = await db.Reservations.AnyAsync(r =>
            r.VehicleId == dto.VehicleId &&
            r.PickupDateTime < dto.DropoffDateTime &&
            dto.PickupDateTime < r.DropoffDateTime);

        if (overlap)
            return (false, "Reservation overlaps with existing booking", null);

        // 3. Mapowanie
        var entity = new Reservation
        {
            VehicleId        = dto.VehicleId,
            PickupLocation   = dto.PickupLocation,
            DropoffLocation  = dto.DropoffLocation,
            PickupDateTime   = dto.PickupDateTime,
            DropoffDateTime  = dto.DropoffDateTime,
            CustomerName     = dto.CustomerName,
            CustomerEmail    = dto.CustomerEmail,
            CreatedAt        = DateTime.UtcNow
        };

        db.Reservations.Add(entity);
        await db.SaveChangesAsync();
        return (true, null, entity);
    }
}