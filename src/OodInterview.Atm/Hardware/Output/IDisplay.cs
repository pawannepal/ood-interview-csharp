namespace OodInterview.Atm.Hardware.Output;

/// <summary>
/// Interface for display hardware component.
/// </summary>
public interface IDisplay
{
    /// <summary>
    /// Displays a message to the user on the ATM screen.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void ShowMessage(string message);

    /// <summary>
    /// Returns the currently displayed message.
    /// </summary>
    string? DisplayedMessage { get; }
}
