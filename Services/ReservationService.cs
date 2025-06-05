using AutoRentNet.Data;
using AutoRentNet.Dtos;
using AutoRentNet.Models;
using Microsoft.EntityFrameworkCore;

public static class ReservationLogic
{
    public static async Task<(bool ok, string? error, Reservation? entity)> CreateAsync(
        ReservationDto dto, AppDbContext db)
    {
        if (dto.StartDate >= dto.EndDate)
            return (false, "StartDate must be earlier than EndDate", null);

        var overlaps = await db.Reservations.AnyAsync(r =>
            r.VehicleId == dto.VehicleId &&
            r.StartDate < dto.EndDate &&
            dto.StartDate < r.EndDate);

        if (overlaps)
            return (false, "Reservation overlaps with existing booking", null);

        var entity = new Reservation
        {
            VehicleId     = dto.VehicleId,
            StartDate     = dto.StartDate,
            EndDate       = dto.EndDate,
            CustomerName  = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            CreatedAt     = DateTime.UtcNow
        };
        db.Reservations.Add(entity);
        await db.SaveChangesAsync();
        return (true, null, entity);
    }
}