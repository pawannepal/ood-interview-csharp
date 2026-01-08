using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Spot;

/// <summary>
/// Represents a handicapped parking spot.
/// </summary>
public class HandicappedSpot : IParkingSpot
{
    private IVehicle? _vehicle;

    public HandicappedSpot(int spotNumber)
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
