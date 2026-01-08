namespace OodInterview.Atm.Hardware.Output;

/// <summary>
/// Simulated cash dispenser for testing purposes.
/// </summary>
public class SimulatedCashDispenser : ICashDispenser
{
    /// <summary>
    /// In the real implementation, this would control the hardware to dispense the cash of given amount.
    /// </summary>
    public void DispenseCash(decimal amount)
    {
        // In a real implementation, this would control hardware to dispense cash
    }
}
