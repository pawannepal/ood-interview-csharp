namespace OodInterview.Restaurant.Command;

/// <summary>
/// Interface for the Command pattern that defines the contract for all order-related commands.
/// </summary>
public interface IOrderCommand
{
    void Execute();
}
