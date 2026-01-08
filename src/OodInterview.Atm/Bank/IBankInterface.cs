using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Bank;

/// <summary>
/// Defines the contract for bank operations that must be implemented by any bank implementation.
/// </summary>
public interface IBankInterface
{
    /// <summary>
    /// Adds a new account to the bank.
    /// </summary>
    void AddAccount(string accountNumber, AccountType type, string cardNumber, string pin);

    /// <summary>
    /// Validates if a card number exists in the bank's records.
    /// </summary>
    bool ValidateCard(string cardNumber);

    /// <summary>
    /// Verifies if the provided PIN matches the card's stored PIN.
    /// </summary>
    bool CheckPin(string cardNumber, string pinNumber);

    /// <summary>
    /// Retrieves account by account number.
    /// </summary>
    Account? GetAccountByAccountNumber(string accountNumber);

    /// <summary>
    /// Retrieves account by card number.
    /// </summary>
    Account? GetAccountByCard(string cardNumber);

    /// <summary>
    /// Attempts to withdraw specified amount from account if sufficient funds exist.
    /// </summary>
    bool WithdrawFunds(Account account, decimal amount);
}
