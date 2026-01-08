namespace OodInterview.Atm.States;

/// <summary>
/// ATMState is a base class that defines the state of the ATM.
/// This class contains the default behavior for each state, which usually
/// renders a message to the display and does not perform any action.
/// </summary>
public class AtmState
{
    /// <summary>
    /// Displays an invalid action message on the ATM screen.
    /// </summary>
    private static void RenderDefaultAction(AtmMachine atmMachine)
    {
        atmMachine.Display.ShowMessage("Invalid action, please try again.");
    }

    /// <summary>
    /// Default implementation for card insertion.
    /// </summary>
    public virtual void ProcessCardInsertion(AtmMachine atmMachine, string cardNumber)
    {
        RenderDefaultAction(atmMachine);
    }

    /// <summary>
    /// Default implementation for card ejection.
    /// </summary>
    public virtual void ProcessCardEjection(AtmMachine atmMachine)
    {
        RenderDefaultAction(atmMachine);
    }

    /// <summary>
    /// Default implementation for PIN entry.
    /// </summary>
    public virtual void ProcessPinEntry(AtmMachine atmMachine, string pin)
    {
        RenderDefaultAction(atmMachine);
    }

    /// <summary>
    /// Default implementation for withdrawal request.
    /// </summary>
    public virtual void ProcessWithdrawalRequest(AtmMachine atmMachine)
    {
        RenderDefaultAction(atmMachine);
    }

    /// <summary>
    /// Default implementation for deposit request.
    /// </summary>
    public virtual void ProcessDepositRequest(AtmMachine atmMachine)
    {
        RenderDefaultAction(atmMachine);
    }

    /// <summary>
    /// Default implementation for amount entry.
    /// </summary>
    public virtual void ProcessAmountEntry(AtmMachine atmMachine, decimal amount)
    {
        RenderDefaultAction(atmMachine);
    }

    /// <summary>
    /// Default implementation for deposit collection.
    /// </summary>
    public virtual void ProcessDepositCollection(AtmMachine atmMachine, decimal amount)
    {
        RenderDefaultAction(atmMachine);
    }
}
