namespace OodInterview.Atm.Hardware.Output;

/// <summary>
/// Console display implementation for testing purposes.
/// </summary>
public class ConsoleDisplay : IDisplay
{
    private string? _message;

    /// <summary>
    /// Shows a message on the display.
    /// </summary>
    public void ShowMessage(string message)
    {
        _message = message;
    }

    /// <summary>
    /// Returns the currently stored message.
    /// </summary>
    public string? DisplayedMessage => _message;
}
