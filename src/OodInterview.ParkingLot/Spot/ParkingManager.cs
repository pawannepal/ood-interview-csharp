using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Spot;

/// <summary>
/// Manages parking spots and vehicle assignments.
/// </summary>
public class ParkingManager
{
    private readonly Dictionary<VehicleSize, List<IParkingSpot>> _availableSpots;
    private readonly Dictionary<IVehicle, IParkingSpot> _vehicleToSpotMap = [];

    /// <summary>
    /// Creates a new parking manager with available spots.
    /// </summary>
    /// <param name="availableSpots">Map of vehicle sizes to available parking spots.</param>
    public ParkingManager(Dictionary<VehicleSize, List<IParkingSpot>> availableSpots)
    {
        _availableSpots = availableSpots;
    }

    /// <summary>
    /// Finds a suitable spot for a vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to find a spot for.</param>
    /// <returns>An available parking spot, or null if none found.</returns>
    public IParkingSpot? FindSpotForVehicle(IVehicle vehicle)
    {
        var vehicleSize = vehicle.Size;

        // Start looking from the smallest spot that can fit the vehicle
        foreach (var size in Enum.GetValues<VehicleSize>())
        {
            if ((int)size >= (int)vehicleSize && _availableSpots.TryGetValue(size, out var spots))
            {
                foreach (var spot in spots)
                {
                    if (spot.IsAvailable)
                    {
                        return spot;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Parks a vehicle in an available spot.
    /// </summary>
    /// <param name="vehicle">The vehicle to park.</param>
    /// <returns>The parking spot assigned, or null if no spot available.</returns>
    public IParkingSpot? ParkVehicle(IVehicle vehicle)
    {
        var spot = FindSpotForVehicle(vehicle);
        if (spot != null)
        {
            spot.Occupy(vehicle);
            _vehicleToSpotMap[vehicle] = spot;
            _availableSpots[spot.Size].Remove(spot);
            return spot;
        }
        return null;
    }

    /// <summary>
    /// Removes a vehicle from its parking spot.
    /// </summary>
    /// <param name="vehicle">The vehicle to unpark.</param>
    public void UnparkVehicle(IVehicle vehicle)
    {
        if (_vehicleToSpotMap.TryGetValue(vehicle, out var spot))
        {
            _vehicleToSpotMap.Remove(vehicle);
            spot.Vacate();
            _availableSpots[spot.Size].Add(spot);
        }
    }

    /// <summary>
    /// Finds the parking spot where a vehicle is parked.
    /// </summary>
    /// <param name="vehicle">The vehicle to find.</param>
    /// <returns>The parking spot, or null if not found.</returns>
    public IParkingSpot? FindVehicleSpot(IVehicle vehicle)
    {
        return _vehicleToSpotMap.GetValueOrDefault(vehicle);
    }
}
