namespace OodInterview.Restaurant.Reservation;

/// <summary>
/// Represents the status of an order item.
/// </summary>
public enum OrderStatus
{
    Pending,
    SentToKitchen,
    Delivered,
    Canceled
}
