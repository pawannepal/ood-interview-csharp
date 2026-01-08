using OodInterview.Restaurant.Menu;
using OodInterview.Restaurant.Reservation;

namespace OodInterview.Restaurant.Table;

/// <summary>
/// Represents a table in the restaurant.
/// </summary>
public class Table
{
    private readonly Dictionary<DateTime, Reservation.Reservation> _reservations = [];
    private readonly Dictionary<MenuItem, List<OrderItem>> _orderedItems = [];

    public Table(int tableId, int capacity)
    {
        TableId = tableId;
        Capacity = capacity;
    }

    public int TableId { get; }
    public int Capacity { get; }

    /// <summary>
    /// Calculates the total bill amount for all ordered items.
    /// </summary>
    public decimal CalculateBillAmount()
    {
        return _orderedItems.Values
            .SelectMany(list => list)
            .Sum(orderItem => orderItem.Item.Price);
    }

    /// <summary>
    /// Adds multiple orders of the same menu item.
    /// </summary>
    public void AddOrder(MenuItem item, int quantity)
    {
        for (var i = 0; i < quantity; i++)
        {
            AddOrder(item);
        }
    }

    /// <summary>
    /// Adds a single menu item to the table's order.
    /// </summary>
    public void AddOrder(MenuItem item)
    {
        if (!_orderedItems.TryGetValue(item, out var orderItems))
        {
            orderItems = [];
            _orderedItems[item] = orderItems;
        }
        orderItems.Add(new OrderItem(item));
    }

    /// <summary>
    /// Removes a menu item from the table's order.
    /// </summary>
    public void RemoveOrder(MenuItem item)
    {
        if (_orderedItems.TryGetValue(item, out var orderItems) && orderItems.Count > 0)
        {
            orderItems.RemoveAt(0);
            if (orderItems.Count == 0)
            {
                _orderedItems.Remove(item);
            }
        }
    }

    /// <summary>
    /// Checks if the table is available at a specific time.
    /// </summary>
    public bool IsAvailableAt(DateTime reservationTime)
    {
        return !_reservations.ContainsKey(reservationTime);
    }

    /// <summary>
    /// Adds a reservation to this table.
    /// </summary>
    public void AddReservation(Reservation.Reservation reservation)
    {
        _reservations[reservation.Time] = reservation;
    }

    /// <summary>
    /// Removes a reservation from this table.
    /// </summary>
    public void RemoveReservation(DateTime reservationTime)
    {
        _reservations.Remove(reservationTime);
    }

    /// <summary>
    /// Returns the current number of guests seated at this table.
    /// </summary>
    public int CurrentPartySize => _reservations.Values.Sum(r => r.PartySize);

    /// <summary>
    /// Returns the map of ordered items.
    /// </summary>
    public IReadOnlyDictionary<MenuItem, List<OrderItem>> OrderedItems => _orderedItems;

    public override string ToString() => $"Table #{TableId} (Capacity: {Capacity})";
}
