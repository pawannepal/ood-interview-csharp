namespace OodInterview.ParkingLot.Fare;

/// <summary>
/// Interface for fare calculation strategies.
/// </summary>
public interface IFareStrategy
{
    /// <summary>
    /// Calculates the fare based on the ticket and input fare.
    /// </summary>
    /// <param name="ticket">The parking ticket.</param>
    /// <param name="inputFare">The current calculated fare.</param>
    /// <returns>The updated fare.</returns>
    decimal CalculateFare(Ticket ticket, decimal inputFare);
}
