using OodInterview.MovieTicket.Location;
using OodInterview.MovieTicket.Showing;

namespace OodInterview.MovieTicket.Ticket;

/// <summary>
/// Represents a movie ticket.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Creates a new ticket for a screening.
    /// </summary>
    /// <param name="screening">The screening for this ticket.</param>
    /// <param name="seat">The seat for this ticket.</param>
    /// <param name="price">The price of this ticket.</param>
    public Ticket(Screening screening, Seat seat, decimal price)
    {
        Screening = screening;
        Seat = seat;
        Price = price;
    }

    /// <summary>
    /// Gets the screening for this ticket.
    /// </summary>
    public Screening Screening { get; }

    /// <summary>
    /// Gets the seat for this ticket.
    /// </summary>
    public Seat Seat { get; }

    /// <summary>
    /// Gets the ticket price.
    /// </summary>
    public decimal Price { get; }
}
