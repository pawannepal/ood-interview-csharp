using OodInterview.Atm.Bank;

namespace OodInterview.Atm.States;

/// <summary>
/// Represents the deposit collection state of the ATM.
/// </summary>
public class DepositCollectionState : AtmState
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
    /// Processes deposit by updating account balance and returning to transaction selection.
    /// </summary>
    public override void ProcessDepositCollection(AtmMachine atmMachine, decimal amount)
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

        var transaction = new DepositTransaction(account, amount);
        transaction.ExecuteTransaction();
        
        atmMachine.Display.ShowMessage($"Deposit successful. Deposited amount: {amount} to account: {account.AccountNumber}");
        atmMachine.TransitionToState(new TransactionSelectionState());
    }
}
