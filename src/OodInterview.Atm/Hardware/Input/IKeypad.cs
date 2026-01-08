using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Hardware.Input;

/// <summary>
/// Interface for keypad hardware component.
/// </summary>
public interface IKeypad
{
    /// <summary>
    /// Handles PIN entry from user.
    /// </summary>
    /// <param name="pin">The PIN entered.</param>
    /// <param name="machine">The ATM machine to notify.</param>
    void HandlePinEntry(string pin, AtmMachine machine);

    /// <summary>
    /// Handles amount entry from user.
    /// </summary>
    /// <param name="amount">The amount entered.</param>
    /// <param name="machine">The ATM machine to notify.</param>
    void HandleAmountEntry(decimal amount, AtmMachine machine);

    /// <summary>
    /// Handles transaction type selection from user.
    /// </summary>
    /// <param name="transactionType">The selected transaction type.</param>
    /// <param name="atmMachine">The ATM machine to notify.</param>
    void HandleSelectTransaction(TransactionType transactionType, AtmMachine atmMachine);
}
