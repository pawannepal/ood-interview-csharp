using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Spot;

/// <summary>
/// Represents an oversized parking spot for large vehicles.
/// </summary>
public class OversizedSpot : IParkingSpot
{
    private IVehicle? _vehicle;

    public OversizedSpot(int spotNumber)
    {
        SpotNumber = spotNumber;
    }

    public int SpotNumber { get; }

    public VehicleSize Size => VehicleSize.Large;

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
