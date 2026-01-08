using System.Security.Cryptography;
using System.Text;
using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Bank;

/// <summary>
/// Represents a bank account with balance, card details, and PIN security.
/// </summary>
public class Account
{
    private decimal _balance;
    private readonly string _accountNumber;
    private readonly string _cardNumber;
    private readonly byte[] _cardPinHash;
    private readonly AccountType _accountType;

    /// <summary>
    /// Creates a new account with initial zero balance and hashed PIN.
    /// </summary>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="type">The type of account.</param>
    /// <param name="cardNumber">The card number associated with the account.</param>
    /// <param name="pin">The PIN for the account.</param>
    public Account(string accountNumber, AccountType type, string cardNumber, string pin)
    {
        _accountNumber = accountNumber;
        _accountType = type;
        _cardNumber = cardNumber;
        _cardPinHash = CalculateMd5(pin);
        _balance = 0;
    }

    /// <summary>
    /// Calculates MD5 hash of the PIN for secure storage.
    /// </summary>
    private static byte[] CalculateMd5(string pinNumber)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(Encoding.UTF8.GetBytes(pinNumber));
    }

    /// <summary>
    /// Validates the entered PIN against stored hash.
    /// </summary>
    /// <param name="pinNumber">The PIN to validate.</param>
    /// <returns>True if the PIN is correct; otherwise, false.</returns>
    public bool ValidatePin(string pinNumber)
    {
        var entryPinHash = CalculateMd5(pinNumber);
        return _cardPinHash.SequenceEqual(entryPinHash);
    }

    /// <summary>
    /// Updates account balance by adding the specified amount.
    /// </summary>
    /// <param name="balanceChange">The amount to add (can be negative for withdrawals).</param>
    public void UpdateBalanceWithTransaction(decimal balanceChange)
    {
        _balance += balanceChange;
    }

    /// <summary>
    /// Gets the current account balance.
    /// </summary>
    public decimal Balance => _balance;

    /// <summary>
    /// Gets the account number.
    /// </summary>
    public string AccountNumber => _accountNumber;

    /// <summary>
    /// Gets the associated card number.
    /// </summary>
    public string CardNumber => _cardNumber;

    /// <summary>
    /// Gets the account type (e.g., SAVING, CHECKING).
    /// </summary>
    public AccountType AccountType => _accountType;
}
