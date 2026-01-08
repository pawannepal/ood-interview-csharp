namespace OodInterview.VendingMachine;

/// <summary>
/// Handles payment processing for the vending machine.
/// </summary>
public class PaymentProcessor
{
    private decimal _currentBalance;

    /// <summary>
    /// Gets the current balance.
    /// </summary>
    public decimal CurrentBalance => _currentBalance;

    /// <summary>
    /// Adds money to the current balance.
    /// </summary>
    /// <param name="amount">The amount to add.</param>
    public void AddBalance(decimal amount)
    {
        _currentBalance += amount;
    }

    /// <summary>
    /// Charges the specified amount from the current balance.
    /// </summary>
    /// <param name="amount">The amount to charge.</param>
    public void Charge(decimal amount)
    {
        _currentBalance -= amount;
    }

    /// <summary>
    /// Returns the remaining change and resets the balance.
    /// </summary>
    /// <returns>The change amount.</returns>
    public decimal ReturnChange()
    {
        decimal change = _currentBalance;
        _currentBalance = 0;
        return change;
    }
}
