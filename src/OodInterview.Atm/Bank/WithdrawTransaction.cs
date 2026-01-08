using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Bank;

/// <summary>
/// Handles the withdrawal transaction process for removing funds from an account.
/// </summary>
public class WithdrawTransaction : ITransaction
{
    private readonly Account _account;
    private readonly decimal _amount;

    /// <summary>
    /// Creates a new withdrawal transaction, throws exception if validation fails.
    /// </summary>
    /// <param name="account">The account to withdraw from.</param>
    /// <param name="amount">The amount to withdraw.</param>
    public WithdrawTransaction(Account account, decimal amount)
    {
        _account = account;
        _amount = amount;
        
        if (!ValidateTransaction())
        {
            throw new InvalidOperationException("Cannot complete withdrawal: Insufficient funds in account");
        }
    }

    /// <summary>
    /// Returns the transaction type as WITHDRAW.
    /// </summary>
    public TransactionType Type => TransactionType.Withdraw;

    /// <summary>
    /// Validates if the account has sufficient funds for withdrawal.
    /// </summary>
    public bool ValidateTransaction()
    {
        return _account.Balance > _amount;
    }

    /// <summary>
    /// Executes the withdrawal by subtracting the amount from account balance.
    /// </summary>
    public void ExecuteTransaction()
    {
        _account.UpdateBalanceWithTransaction(-_amount);
    }
}
