using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Bank;

/// <summary>
/// Manages bank accounts and provides banking operations like validation and transactions.
/// </summary>
public class Bank : IBankInterface
{
    private readonly Dictionary<string, Account> _accounts = new();
    private readonly Dictionary<string, Account> _accountByCard = new();

    /// <summary>
    /// Creates a new account and stores it in both account and card maps.
    /// </summary>
    public void AddAccount(string accountNumber, AccountType type, string cardNumber, string pin)
    {
        var newAccount = new Account(accountNumber, type, cardNumber, pin);
        _accounts[newAccount.AccountNumber] = newAccount;
        _accountByCard[newAccount.CardNumber] = newAccount;
    }

    /// <summary>
    /// Checks if a card number exists in the bank's records.
    /// </summary>
    public bool ValidateCard(string cardNumber)
    {
        return GetAccountByCard(cardNumber) != null;
    }

    /// <summary>
    /// Verifies if the provided PIN matches the card's stored PIN.
    /// </summary>
    public bool CheckPin(string cardNumber, string pinNumber)
    {
        var account = GetAccountByCard(cardNumber);
        return account?.ValidatePin(pinNumber) ?? false;
    }

    /// <summary>
    /// Retrieves account by account number.
    /// </summary>
    public Account? GetAccountByAccountNumber(string accountNumber)
    {
        return _accounts.GetValueOrDefault(accountNumber);
    }

    /// <summary>
    /// Retrieves account by card number.
    /// </summary>
    public Account? GetAccountByCard(string cardNumber)
    {
        return _accountByCard.GetValueOrDefault(cardNumber);
    }

    /// <summary>
    /// Attempts to withdraw specified amount from account if sufficient funds exist.
    /// </summary>
    public bool WithdrawFunds(Account account, decimal amount)
    {
        if (account.Balance >= amount)
        {
            account.UpdateBalanceWithTransaction(-amount);
            return true;
        }
        return false;
    }
}
