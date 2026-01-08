using OodInterview.Restaurant.Reservation;

namespace OodInterview.Restaurant.Command;

/// <summary>
/// Command that handles sending order items to the kitchen.
/// </summary>
public class SendToKitchenCommand : IOrderCommand
{
    private readonly OrderItem _orderItem;

    public SendToKitchenCommand(OrderItem orderItem)
    {
        _orderItem = orderItem;
    }

    public void Execute()
    {
        _orderItem.SendToKitchen();
    }
}
