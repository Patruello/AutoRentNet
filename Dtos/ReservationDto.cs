namespace AutoRentNet.Dtos;

public record ReservationDto(
    int VehicleId,
    DateTime StartDate,
    DateTime EndDate,
    string CustomerName,
    string CustomerEmail);