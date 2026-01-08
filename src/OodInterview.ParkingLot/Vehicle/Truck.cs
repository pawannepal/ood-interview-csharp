namespace OodInterview.ParkingLot.Vehicle;

/// <summary>
/// Represents a truck (large-sized vehicle).
/// </summary>
public class Truck : IVehicle
{
    public Truck(string licensePlate)
    {
        LicensePlate = licensePlate;
    }

    public string LicensePlate { get; }

    public VehicleSize Size => VehicleSize.Large;
}
