namespace OodInterview.Atm.Hardware.Output;

/// <summary>
/// Interface for cash dispenser hardware component.
/// </summary>
public interface ICashDispenser
{
    /// <summary>
    /// Dispenses the specified amount of cash to the user.
    /// </summary>
    /// <param name="amount">The amount of cash to dispense.</param>
    void DispenseCash(decimal amount);
}
