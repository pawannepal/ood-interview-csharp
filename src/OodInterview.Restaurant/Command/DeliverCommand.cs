using OodInterview.Restaurant.Reservation;

namespace OodInterview.Restaurant.Command;

/// <summary>
/// Command that handles delivery of order items to customers.
/// </summary>
public class DeliverCommand : IOrderCommand
{
    private readonly OrderItem _orderItem;

    public DeliverCommand(OrderItem orderItem)
    {
        _orderItem = orderItem;
    }

    public void Execute()
    {
        _orderItem.DeliverToCustomer();
    }
}
