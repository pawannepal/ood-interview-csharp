namespace OodInterview.Atm.States;

/// <summary>
/// Represents the idle state of the ATM when waiting for a card.
/// </summary>
public class IdleState : AtmState
{
    /// <summary>
    /// This method is called when a card is inserted into the ATM.
    /// This transitions the ATM to the PinEntryState if the card is valid.
    /// </summary>
    public override void ProcessCardInsertion(AtmMachine atmMachine, string cardNumber)
    {
        if (atmMachine.BankInterface.ValidateCard(cardNumber))
        {
            atmMachine.Display.ShowMessage("Please enter your PIN");
            atmMachine.TransitionToState(new PinEntryState());
        }
        else
        {
            atmMachine.Display.ShowMessage("Invalid card. Please try again.");
        }
    }
}
