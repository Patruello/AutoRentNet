namespace AutoRentNet.Dtos;

public record ReservationDto(
    int VehicleId,
    string PickupLocation,
    string DropoffLocation,
    DateTime PickupDateTime,
    DateTime DropoffDateTime,
    string CustomerName,
    string CustomerEmail);