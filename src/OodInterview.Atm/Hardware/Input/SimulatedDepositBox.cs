namespace OodInterview.Atm.Hardware.Input;

/// <summary>
/// Simulated deposit box for testing purposes.
/// </summary>
public class SimulatedDepositBox : IDepositBox
{
    /// <summary>
    /// Simulates accepting cash deposit by notifying the ATM machine.
    /// </summary>
    public void AcceptDeposit(decimal amount, AtmMachine atmMachine)
    {
        atmMachine.CollectDeposit(amount);
    }
}
