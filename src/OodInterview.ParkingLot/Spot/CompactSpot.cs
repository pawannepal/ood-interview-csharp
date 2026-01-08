using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Spot;

/// <summary>
/// Represents a compact parking spot for small vehicles.
/// </summary>
public class CompactSpot : IParkingSpot
{
    private IVehicle? _vehicle;

    public CompactSpot(int spotNumber)
    {
        SpotNumber = spotNumber;
    }

    public int SpotNumber { get; }

    public VehicleSize Size => VehicleSize.Small;

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
