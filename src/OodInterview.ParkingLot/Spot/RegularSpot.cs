using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Spot;

/// <summary>
/// Represents a regular parking spot for medium-sized vehicles.
/// </summary>
public class RegularSpot : IParkingSpot
{
    private IVehicle? _vehicle;

    public RegularSpot(int spotNumber)
    {
        SpotNumber = spotNumber;
    }

    public int SpotNumber { get; }

    public VehicleSize Size => VehicleSize.Medium;

    public bool IsAvailable => _vehicle == null;

    public void Occupy(IVehicle vehicle)
    {
        if (IsAvailable)
        {
            _vehicle = vehicle;
        }
    }

    public void Vacate()
    {
        _vehicle = null;
    }
}
