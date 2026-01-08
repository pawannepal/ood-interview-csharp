using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Hardware.Input;

/// <summary>
/// Simulated keypad for testing purposes.
/// </summary>
public class SimulatedKeypad : IKeypad
{
    /// <summary>
    /// Simulates PIN entry by forwarding the input to ATM machine.
    /// </summary>
    public void HandlePinEntry(string pin, AtmMachine atmMachine)
    {
        atmMachine.EnterPin(pin);
    }

    /// <summary>
    /// Simulates amount entry by forwarding the input to ATM machine.
    /// </summary>
    public void HandleAmountEntry(decimal amount, AtmMachine atmMachine)
    {
        atmMachine.EnterAmount(amount);
    }

    /// <summary>
    /// Simulates transaction selection by forwarding the choice to ATM machine.
    /// </summary>
    public void HandleSelectTransaction(TransactionType transactionType, AtmMachine atmMachine)
    {
        switch (transactionType)
        {
            case TransactionType.Withdraw:
                atmMachine.WithdrawRequest();
                break;
            case TransactionType.Deposit:
                atmMachine.DepositRequest();
                break;
            default:
                throw new ArgumentException($"Invalid transaction type: {transactionType}");
        }
    }
}
