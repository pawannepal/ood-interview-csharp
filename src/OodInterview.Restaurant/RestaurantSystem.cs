using OodInterview.Restaurant.Command;
using OodInterview.Restaurant.Menu;
using OodInterview.Restaurant.Reservation;
using OodInterview.Restaurant.Table;

namespace OodInterview.Restaurant;

/// <summary>
/// Main restaurant class that manages reservations, orders and tables.
/// Uses the Command Pattern for order management.
/// </summary>
public class RestaurantSystem
{
    private readonly OrderManager _orderManager = new();

    public RestaurantSystem(string name, Menu.Menu menu, Layout layout)
    {
        Name = name;
        Menu = menu;
        Layout = layout;
        ReservationManager = new ReservationManager(layout);
    }

    public string Name { get; }
    public Menu.Menu Menu { get; }
    public Layout Layout { get; }
    public ReservationManager ReservationManager { get; }

    /// <summary>
    /// Finds possible reservation times within a time range.
    /// </summary>
    public DateTime[] FindAvailableTimeSlots(DateTime rangeStart, DateTime rangeEnd, int partySize)
    {
        return ReservationManager.FindAvailableTimeSlots(rangeStart, rangeEnd, partySize);
    }

    /// <summary>
    /// Creates a reservation for a party at the specified time.
    /// </summary>
    public Reservation.Reservation CreateScheduledReservation(string partyName, int partySize, DateTime time)
    {
        return ReservationManager.CreateReservation(partyName, partySize, time);
    }

    /// <summary>
    /// Removes an existing reservation.
    /// </summary>
    public void RemoveReservation(string partyName, int partySize, DateTime reservationTime)
    {
        ReservationManager.RemoveReservation(partyName, partySize, reservationTime);
    }

    /// <summary>
    /// Creates a reservation for a walk-in party.
    /// </summary>
    public Reservation.Reservation CreateWalkInReservation(string partyName, int partySize)
    {
        return ReservationManager.CreateReservation(partyName, partySize, DateTime.Now);
    }

    /// <summary>
    /// Adds an item to a table's order and sends it to the kitchen.
    /// </summary>
    public void OrderItem(Table.Table table, MenuItem item)
    {
        table.AddOrder(item);
        
        // Get the last added order item and send to kitchen
        if (table.OrderedItems.TryGetValue(item, out var orderItems) && orderItems.Count > 0)
        {
            var lastOrder = orderItems[^1];
            var sendToKitchen = new SendToKitchenCommand(lastOrder);
            _orderManager.AddCommand(sendToKitchen);
            _orderManager.ExecuteCommands();
        }
    }

    /// <summary>
    /// Cancels an order for an item at a table.
    /// </summary>
    public void CancelItem(Table.Table table, MenuItem item)
    {
        if (table.OrderedItems.TryGetValue(item, out var orderItems) && orderItems.Count > 0)
        {
            var lastOrder = orderItems[^1];
            var cancelOrder = new CancelCommand(lastOrder);
            _orderManager.AddCommand(cancelOrder);
            _orderManager.ExecuteCommands();
            table.RemoveOrder(item);
        }
    }

    /// <summary>
    /// Delivers an item to the customer at a table.
    /// </summary>
    public void DeliverItem(Table.Table table, MenuItem item)
    {
        if (table.OrderedItems.TryGetValue(item, out var orderItems) && orderItems.Count > 0)
        {
            var lastOrder = orderItems[^1];
            var deliverOrder = new DeliverCommand(lastOrder);
            _orderManager.AddCommand(deliverOrder);
            _orderManager.ExecuteCommands();
        }
    }

    /// <summary>
    /// Calculates the bill amount for a table.
    /// </summary>
    public decimal CalculateTableBill(Table.Table table)
    {
        return table.CalculateBillAmount();
    }
}
