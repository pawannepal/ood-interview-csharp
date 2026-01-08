namespace OodInterview.VendingMachine;

/// <summary>
/// Main vending machine class that orchestrates product selection, payment, and dispensing.
/// </summary>
public class VendingMachine
{
    private readonly List<Transaction> _transactionHistory;
    private readonly InventoryManager _inventoryManager;
    private readonly PaymentProcessor _paymentProcessor;
    private Transaction _currentTransaction;

    /// <summary>
    /// Initializes a new instance of the VendingMachine class.
    /// </summary>
    public VendingMachine()
    {
        _transactionHistory = new List<Transaction>();
        _currentTransaction = new Transaction();
        _inventoryManager = new InventoryManager();
        _paymentProcessor = new PaymentProcessor();
    }

    /// <summary>
    /// Sets the rack configuration for the vending machine.
    /// </summary>
    /// <param name="racks">The rack configuration.</param>
    public void SetRack(Dictionary<string, Rack> racks)
    {
        _inventoryManager.UpdateRack(racks);
    }

    /// <summary>
    /// Inserts money into the vending machine.
    /// </summary>
    /// <param name="amount">The amount to insert.</param>
    public void InsertMoney(decimal amount)
    {
        _paymentProcessor.AddBalance(amount);
    }

    /// <summary>
    /// Selects a product by its rack code.
    /// </summary>
    /// <param name="rackId">The rack code (e.g., "A1", "A2").</param>
    public void ChooseProduct(string rackId)
    {
        var product = _inventoryManager.GetProductInRack(rackId);
        _currentTransaction.Rack = _inventoryManager.GetRack(rackId);
        _currentTransaction.Product = product;
    }

    /// <summary>
    /// Confirms and processes the current transaction.
    /// </summary>
    /// <returns>The completed transaction with change information.</returns>
    /// <exception cref="InvalidTransactionException">
    /// Thrown when product is not selected, out of stock, or insufficient funds.
    /// </exception>
    public Transaction ConfirmTransaction()
    {
        // Step 1: Validate the transaction before processing
        ValidateTransaction();

        // Step 2: Charge the customer for the product
        _paymentProcessor.Charge(_currentTransaction.Product!.UnitPrice);

        // Step 3: Dispense the product from the rack
        _inventoryManager.DispenseProductFromRack(_currentTransaction.Rack!);

        // Step 4: Return the change to the customer
        _currentTransaction.TotalAmount = _paymentProcessor.ReturnChange();

        // Step 5: Add the completed transaction to the history
        _transactionHistory.Add(_currentTransaction);
        var completedTransaction = _currentTransaction;

        // Reset the current transaction for the next purchase
        _currentTransaction = new Transaction();
        return completedTransaction;
    }

    private void ValidateTransaction()
    {
        if (_currentTransaction.Product == null)
        {
            throw new InvalidTransactionException("Invalid product selection");
        }
        else if (_currentTransaction.Rack!.ProductCount == 0)
        {
            throw new InvalidTransactionException("Insufficient inventory for product.");
        }
        else if (_paymentProcessor.CurrentBalance < _currentTransaction.Product.UnitPrice)
        {
            throw new InvalidTransactionException("Insufficient fund");
        }
    }

    /// <summary>
    /// Gets the transaction history.
    /// </summary>
    /// <returns>An immutable list of completed transactions.</returns>
    public IReadOnlyList<Transaction> GetTransactionHistory()
    {
        return _transactionHistory.AsReadOnly();
    }

    /// <summary>
    /// Cancels the current transaction and returns any inserted money.
    /// </summary>
    public void CancelTransaction()
    {
        _paymentProcessor.ReturnChange();
        _currentTransaction = new Transaction();
    }

    /// <summary>
    /// Gets the inventory manager for checking stock levels.
    /// </summary>
    /// <returns>The inventory manager.</returns>
    public InventoryManager GetInventoryManager()
    {
        return _inventoryManager;
    }
}
