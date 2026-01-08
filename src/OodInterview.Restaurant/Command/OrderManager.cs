namespace OodInterview.Restaurant.Command;

/// <summary>
/// Invoker that manages and executes order commands.
/// </summary>
public class OrderManager
{
    private readonly List<IOrderCommand> _commandQueue = [];

    /// <summary>
    /// Adds a command to the queue for later execution.
    /// </summary>
    public void AddCommand(IOrderCommand command)
    {
        _commandQueue.Add(command);
    }

    /// <summary>
    /// Executes all commands in the queue and clears it.
    /// </summary>
    public void ExecuteCommands()
    {
        foreach (var command in _commandQueue)
        {
            command.Execute();
        }
        _commandQueue.Clear();
    }
}
