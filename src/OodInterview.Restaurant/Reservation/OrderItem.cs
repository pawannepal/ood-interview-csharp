using OodInterview.Restaurant.Menu;

namespace OodInterview.Restaurant.Reservation;

/// <summary>
/// Represents a food item ordered by a customer with its current status.
/// </summary>
public class OrderItem
{
    public OrderItem(MenuItem item)
    {
        Item = item;
        Status = OrderStatus.Pending;
    }

    public MenuItem Item { get; }
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Updates the status to indicate the item has been sent to the kitchen.
    /// </summary>
    public void SendToKitchen()
    {
        if (Status == OrderStatus.Pending)
        {
            Status = OrderStatus.SentToKitchen;
        }
    }

    /// <summary>
    /// Updates the status to indicate the item has been delivered to the customer.
    /// </summary>
    public void DeliverToCustomer()
    {
        if (Status == OrderStatus.SentToKitchen)
        {
            Status = OrderStatus.Delivered;
        }
    }

    /// <summary>
    /// Updates the status to indicate the item has been canceled.
    /// </summary>
    public void Cancel()
    {
        if (Status is OrderStatus.Pending or OrderStatus.SentToKitchen)
        {
            Status = OrderStatus.Canceled;
        }
    }
}
