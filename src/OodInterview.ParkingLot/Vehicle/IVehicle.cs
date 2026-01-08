namespace OodInterview.ParkingLot.Vehicle;

/// <summary>
/// Interface representing a vehicle that can park in the lot.
/// </summary>
public interface IVehicle
{
    /// <summary>
    /// Gets the license plate of the vehicle.
    /// </summary>
    string LicensePlate { get; }

    /// <summary>
    /// Gets the size of the vehicle.
    /// </summary>
    VehicleSize Size { get; }
}
