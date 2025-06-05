namespace AutoRentNet.Dtos;

public record ReservationReadDto(int Id,
    int VehicleId,
    string VehicleName,
    DateTime StartDate,
    DateTime EndDate,
    string CustomerName);