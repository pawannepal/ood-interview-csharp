namespace OodInterview.ParkingLot.Fare;

/// <summary>
/// Calculates the total parking fare using multiple strategies.
/// </summary>
public class FareCalculator
{
    private readonly List<IFareStrategy> _fareStrategies;

    public FareCalculator(List<IFareStrategy> fareStrategies)
    {
        _fareStrategies = fareStrategies;
    }

    /// <summary>
    /// Calculates the total fare for a ticket.
    /// </summary>
    /// <param name="ticket">The parking ticket.</param>
    /// <returns>The total fare.</returns>
    public decimal CalculateFare(Ticket ticket)
    {
        var fare = 0m;
        foreach (var strategy in _fareStrategies)
        {
            fare = strategy.CalculateFare(ticket, fare);
        }
        return fare;
    }
}
