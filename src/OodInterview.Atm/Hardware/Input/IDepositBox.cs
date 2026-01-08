namespace OodInterview.Atm.Hardware.Input;

/// <summary>
/// Interface for handling cash deposits into the ATM.
/// </summary>
public interface IDepositBox
{
    /// <summary>
    /// Accepts cash deposit and notifies ATM of the deposited amount.
    /// </summary>
    /// <param name="amount">The amount being deposited.</param>
    /// <param name="machine">The ATM machine to notify.</param>
    void AcceptDeposit(decimal amount, AtmMachine machine);
}
