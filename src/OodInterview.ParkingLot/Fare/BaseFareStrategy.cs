using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Fare;

/// <summary>
/// Base fare strategy that calculates fare based on vehicle size and duration.
/// </summary>
public class BaseFareStrategy : IFareStrategy
{
    private const decimal SmallVehicleRate = 1.0m;
    private const decimal MediumVehicleRate = 2.0m;
    private const decimal LargeVehicleRate = 3.0m;

    public decimal CalculateFare(Ticket ticket, decimal inputFare)
    {
        var rate = ticket.Vehicle.Size switch
        {
            VehicleSize.Medium => MediumVehicleRate,
            VehicleSize.Large => LargeVehicleRate,
            _ => SmallVehicleRate
        };

        return inputFare + (rate * ticket.CalculateParkingDuration());
    }
}
