using OodInterview.MovieTicket.Rate;

namespace OodInterview.MovieTicket.Location;

/// <summary>
/// Represents a seat in a cinema room.
/// </summary>
public class Seat
{
    /// <summary>
    /// Creates a new seat with the specified seat number and pricing strategy.
    /// </summary>
    /// <param name="seatNumber">The seat number (e.g., "0-0").</param>
    /// <param name="pricingStrategy">The pricing strategy for this seat.</param>
    public Seat(string seatNumber, IPricingStrategy? pricingStrategy)
    {
        SeatNumber = seatNumber;
        PricingStrategy = pricingStrategy;
    }

    /// <summary>
    /// Gets the seat number.
    /// </summary>
    public string SeatNumber { get; }

    /// <summary>
    /// Gets or sets the pricing strategy for this seat.
    /// </summary>
    public IPricingStrategy? PricingStrategy { get; set; }
}
