using OodInterview.Atm.Bank.Enums;

namespace OodInterview.Atm.Bank;

/// <summary>
/// Base interface for all types of bank transactions with common operations.
/// </summary>
public interface ITransaction
{
    /// <summary>
    /// Gets the type of the transaction.
    /// </summary>
    TransactionType Type { get; }

    /// <summary>
    /// Validates if the transaction can be executed.
    /// </summary>
    /// <returns>True if the transaction is valid; otherwise, false.</returns>
    bool ValidateTransaction();

    /// <summary>
    /// Executes the transaction.
    /// </summary>
    void ExecuteTransaction();
}
