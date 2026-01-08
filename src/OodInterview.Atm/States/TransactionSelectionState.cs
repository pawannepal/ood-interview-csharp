namespace OodInterview.Atm.States;

/// <summary>
/// Represents the transaction selection state of the ATM.
/// </summary>
public class TransactionSelectionState : AtmState
{
    /// <summary>
    /// Handles card ejection by returning to idle state and canceling transaction.
    /// </summary>
    public override void ProcessCardEjection(AtmMachine atmMachine)
    {
        atmMachine.Display.ShowMessage("Card ejected, transaction cancelled.");
        atmMachine.TransitionToState(new IdleState());
    }

    /// <summary>
    /// Initiates withdrawal process by transitioning to amount entry state.
    /// </summary>
    public override void ProcessWithdrawalRequest(AtmMachine atmMachine)
    {
        atmMachine.Display.ShowMessage("Enter amount to withdraw:");
        atmMachine.TransitionToState(new WithdrawAmountEntryState());
    }

    /// <summary>
    /// Initiates deposit process by transitioning to deposit collection state.
    /// </summary>
    public override void ProcessDepositRequest(AtmMachine atmMachine)
    {
        atmMachine.Display.ShowMessage("Please deposit cash into the deposit box.");
        atmMachine.TransitionToState(new DepositCollectionState());
    }
}
