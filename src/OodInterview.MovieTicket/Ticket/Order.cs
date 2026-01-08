namespace OodInterview.MovieTicket.Ticket;

/// <summary>
/// Represents an order containing multiple tickets.
/// </summary>
public class Order
{
    private readonly List<Ticket> _tickets = [];

    /// <summary>
    /// Creates a new order with the specified order date.
    /// </summary>
    /// <param name="orderDate">The date of the order.</param>
    public Order(DateTime orderDate)
    {
        OrderDate = orderDate;
    }

    /// <summary>
    /// Gets the order date.
    /// </summary>
    public DateTime OrderDate { get; }

    /// <summary>
    /// Gets all tickets in this order.
    /// </summary>
    public IReadOnlyList<Ticket> Tickets => _tickets.AsReadOnly();

    /// <summary>
    /// Adds a ticket to this order.
    /// </summary>
    /// <param name="ticket">The ticket to add.</param>
    public void AddTicket(Ticket ticket)
    {
        _tickets.Add(ticket);
    }

    /// <summary>
    /// Calculates the total price of all tickets in the order.
    /// </summary>
    /// <returns>The total price.</returns>
    public decimal CalculateTotalPrice()
    {
        return _tickets.Sum(t => t.Price);
    }
}
