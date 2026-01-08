using OodInterview.Restaurant.Reservation;

namespace OodInterview.Restaurant.Command;

/// <summary>
/// Command that handles cancellation of order items.
/// </summary>
public class CancelCommand : IOrderCommand
{
    private readonly OrderItem _orderItem;

    public CancelCommand(OrderItem orderItem)
    {
        _orderItem = orderItem;
    }

    public void Execute()
    {
        _orderItem.Cancel();
    }
}
