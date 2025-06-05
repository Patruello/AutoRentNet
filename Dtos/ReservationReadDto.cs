namespace AutoRentNet.Dtos;

public record ReservationReadDto(
    int Id,
    int VehicleId,
    string VehicleName,
    string PickupLocation,
    string DropoffLocation,
    DateTime PickupDateTime,
    DateTime DropoffDateTime,
    string CustomerName);