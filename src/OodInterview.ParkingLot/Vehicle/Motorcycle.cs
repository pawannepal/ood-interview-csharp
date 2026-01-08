namespace OodInterview.ParkingLot.Vehicle;

/// <summary>
/// Represents a motorcycle (small-sized vehicle).
/// </summary>
public class Motorcycle : IVehicle
{
    public Motorcycle(string licensePlate)
    {
        LicensePlate = licensePlate;
    }

    public string LicensePlate { get; }

    public VehicleSize Size => VehicleSize.Small;
}
