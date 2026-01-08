using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Spot;

/// <summary>
/// Interface representing a parking spot.
/// </summary>
public interface IParkingSpot
{
    /// <summary>
    /// Gets the spot number.
    /// </summary>
    int SpotNumber { get; }

    /// <summary>
    /// Gets the size of vehicles this spot can accommodate.
    /// </summary>
    VehicleSize Size { get; }

    /// <summary>
    /// Gets whether the spot is available.
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// Occupies the spot with a vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to occupy the spot.</param>
    void Occupy(IVehicle vehicle);

    /// <summary>
    /// Vacates the spot.
    /// </summary>
    void Vacate();
}
