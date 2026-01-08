namespace OodInterview.Atm.States;

/// <summary>
/// Represents the PIN entry state of the ATM.
/// </summary>
public class PinEntryState : AtmState
{
    /// <summary>
    /// If the user instead ejects the card, the ATM will release the card and go back to Idle State.
    /// </summary>
    public override void ProcessCardEjection(AtmMachine atmMachine)
    {
        atmMachine.Display.ShowMessage("Card ejected");
        atmMachine.TransitionToState(new IdleState());
    }

    /// <summary>
    /// If the user enters a valid PIN, the ATM will transition to TransactionSelectionState.
    /// Otherwise, it will stay in this state.
    /// </summary>
    public override void ProcessPinEntry(AtmMachine atmMachine, string pin)
    {
        var cardNumber = atmMachine.CardProcessor.CardNumber;

        if (cardNumber == null)
        {
            atmMachine.Display.ShowMessage("No card inserted");
            return;
        }

        var isPinCorrect = atmMachine.BankInterface.CheckPin(cardNumber, pin);

        if (isPinCorrect)
        {
            atmMachine.Display.ShowMessage("PIN correct, select transaction type");
            atmMachine.TransitionToState(new TransactionSelectionState());
        }
        else
        {
            atmMachine.Display.ShowMessage("Invalid PIN. Please try again");
        }
    }
}
