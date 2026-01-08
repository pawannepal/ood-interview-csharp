namespace OodInterview.Atm.Hardware.Input;

/// <summary>
/// Interface for card processor hardware component.
/// </summary>
public interface ICardProcessor
{
    /// <summary>
    /// Handles card insertion by storing card number and notifying ATM.
    /// </summary>
    /// <param name="cardNumber">The card number being inserted.</param>
    /// <param name="atmMachine">The ATM machine to notify.</param>
    void HandleCardInsertion(string cardNumber, AtmMachine atmMachine);

    /// <summary>
    /// Handles card ejection by clearing stored card number and notifying ATM.
    /// </summary>
    /// <param name="atmMachine">The ATM machine to notify.</param>
    void HandleCardEjection(AtmMachine atmMachine);

    /// <summary>
    /// Returns the currently stored card number.
    /// </summary>
    string? CardNumber { get; }
}
