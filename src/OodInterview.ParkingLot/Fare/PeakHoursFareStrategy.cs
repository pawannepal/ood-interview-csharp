namespace OodInterview.ParkingLot.Fare;

/// <summary>
/// Fare strategy that applies a peak hours multiplier.
/// </summary>
public class PeakHoursFareStrategy : IFareStrategy
{
    private const decimal PeakHoursMultiplier = 1.5m;

    public decimal CalculateFare(Ticket ticket, decimal inputFare)
    {
        if (IsPeakHours(ticket.EntryTime))
        {
            return inputFare * PeakHoursMultiplier;
        }
        return inputFare;
    }

    private static bool IsPeakHours(DateTime time)
    {
        var hour = time.Hour;
        return (hour >= 7 && hour <= 10) || (hour >= 16 && hour <= 19);
    }
}
