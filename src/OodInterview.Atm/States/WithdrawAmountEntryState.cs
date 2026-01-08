namespace OodInterview.Atm.States;

/// <summary>
/// Represents the withdraw amount entry state of the ATM.
/// </summary>
public class WithdrawAmountEntryState : AtmState
{
    /// <summary>
    /// Handles card ejection by canceling transaction and returning to idle state.
    /// </summary>
    public override void ProcessCardEjection(AtmMachine atmMachine)
    {
        atmMachine.Display.ShowMessage("Transaction cancelled, card ejected");
        atmMachine.TransitionToState(new IdleState());
    }

    /// <summary>
    /// Processes withdrawal request by checking balance and dispensing cash if sufficient funds.
    /// </summary>
    public override void ProcessAmountEntry(AtmMachine atmMachine, decimal amount)
    {
        var cardNumber = atmMachine.CardProcessor.CardNumber;
        
        if (cardNumber == null)
        {
            atmMachine.Display.ShowMessage("No card inserted");
            return;
        }

        var account = atmMachine.BankInterface.GetAccountByCard(cardNumber);
        
        if (account == null)
        {
            atmMachine.Display.ShowMessage("Account not found");
            atmMachine.TransitionToState(new TransactionSelectionState());
            return;
        }

        var isSuccess = atmMachine.BankInterface.WithdrawFunds(account, amount);

        if (isSuccess)
        {
            atmMachine.CashDispenser.DispenseCash(amount);
            atmMachine.Display.ShowMessage("Please take your cash.");
        }
        else
        {
            atmMachine.Display.ShowMessage("Insufficient funds, please try again.");
        }
        
        atmMachine.TransitionToState(new TransactionSelectionState());
    }
}
