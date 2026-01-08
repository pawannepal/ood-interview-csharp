namespace OodInterview.VendingMachine;

/// <summary>
/// Exception thrown when a transaction is invalid.
/// </summary>
public class InvalidTransactionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the InvalidTransactionException class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public InvalidTransactionException(string message) : base(message)
    {
    }
}
