namespace OodInterview.ParkingLot.Vehicle;

/// <summary>
/// Represents a car (medium-sized vehicle).
/// </summary>
public class Car : IVehicle
{
    public Car(string licensePlate)
    {
        LicensePlate = licensePlate;
    }

    public string LicensePlate { get; }

    public VehicleSize Size => VehicleSize.Medium;
}
