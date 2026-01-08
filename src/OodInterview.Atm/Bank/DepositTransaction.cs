using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Bank;

/// <summary>
/// Handles the deposit transaction process for adding funds to an account.
/// </summary>
public class DepositTransaction : ITransaction
{
    private readonly Account _account;
    private readonly decimal _amount;

    /// <summary>
    /// Creates a new deposit transaction.
    /// </summary>
    /// <param name="account">The account to deposit to.</param>
    /// <param name="amount">The amount to deposit.</param>
    public DepositTransaction(Account account, decimal amount)
    {
        _account = account;
        _amount = amount;
    }

    /// <summary>
    /// Returns the transaction type as DEPOSIT.
    /// </summary>
    public TransactionType Type => TransactionType.Deposit;

    /// <summary>
    /// Deposit transactions are always valid.
    /// </summary>
    public bool ValidateTransaction() => true;

    /// <summary>
    /// Executes the deposit by adding the amount to the account balance.
    /// </summary>
    public void ExecuteTransaction()
    {
        _account.UpdateBalanceWithTransaction(_amount);
    }
}
