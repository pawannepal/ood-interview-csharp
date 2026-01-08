namespace OodInterview.Atm.Hardware.Input;

/// <summary>
/// Simulated card processor for testing purposes.
/// </summary>
public class SimulatedCardProcessor : ICardProcessor
{
    private string? _cardNumber;

    /// <summary>
    /// Simulates card insertion by storing card number and notifying ATM.
    /// </summary>
    public void HandleCardInsertion(string cardNumber, AtmMachine atmMachine)
    {
        _cardNumber = cardNumber;
        atmMachine.InsertCard(cardNumber);
    }

    /// <summary>
    /// Simulates card ejection by clearing stored card number and notifying ATM.
    /// </summary>
    public void HandleCardEjection(AtmMachine atmMachine)
    {
        _cardNumber = null;
        atmMachine.EjectCard();
    }

    /// <summary>
    /// Returns the currently stored card number.
    /// </summary>
    public string? CardNumber => _cardNumber;
}
