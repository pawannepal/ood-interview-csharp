# Vending Machine System

A simple vending machine implementation demonstrating object-oriented design principles with product management, inventory tracking, payment processing, and transaction handling.

## Overview

This project implements a vending machine system with the following features:
- Product management with multiple racks
- Inventory tracking per rack
- Payment processing with balance and change
- Transaction history
- Validation for insufficient funds and out-of-stock scenarios

## Class Diagram

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          VendingMachine                                  │
│─────────────────────────────────────────────────────────────────────────│
│ - _transactionHistory: List<Transaction>                                 │
│ - _inventoryManager: InventoryManager                                    │
│ - _paymentProcessor: PaymentProcessor                                    │
│ - _currentTransaction: Transaction                                       │
│─────────────────────────────────────────────────────────────────────────│
│ + SetRack(racks): void                                                   │
│ + InsertMoney(amount): void                                              │
│ + ChooseProduct(rackId): void                                            │
│ + ConfirmTransaction(): Transaction                                      │
│ + CancelTransaction(): void                                              │
│ + GetTransactionHistory(): IReadOnlyList<Transaction>                    │
│ + GetInventoryManager(): InventoryManager                                │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ uses
            ┌───────────────────────┼───────────────────────┐
            ▼                       ▼                       ▼
┌───────────────────┐   ┌───────────────────┐   ┌───────────────────┐
│InventoryManager   │   │ PaymentProcessor  │   │   Transaction     │
│───────────────────│   │───────────────────│   │───────────────────│
│ - _racks: Dict    │   │ - _currentBalance │   │ + Rack            │
│───────────────────│   │───────────────────│   │ + Product         │
│ + GetProductInRack│   │ + AddBalance()    │   │ + TotalAmount     │
│ + DispenseProduct │   │ + Charge()        │   │                   │
│ + UpdateRack()    │   │ + ReturnChange()  │   │                   │
│ + GetRack()       │   │ + CurrentBalance  │   │                   │
└───────────────────┘   └───────────────────┘   └───────────────────┘
            │
            ▼
    ┌───────────────┐
    │     Rack      │
    │───────────────│
    │ + RackCode    │
    │ + Product     │───────────┐
    │ + ProductCount│           ▼
    │ + SetCount()  │   ┌───────────────┐
    └───────────────┘   │    Product    │
                        │───────────────│
                        │ + ProductCode │
                        │ + Description │
                        │ + UnitPrice   │
                        └───────────────┘

┌───────────────────────────────┐
│  InvalidTransactionException  │
│───────────────────────────────│
│ - Invalid product selection   │
│ - Insufficient inventory      │
│ - Insufficient fund           │
└───────────────────────────────┘
```

## Design Patterns Used

### 1. Facade Pattern

The `VendingMachine` class acts as a facade, providing a simplified interface to the complex subsystems:
- `InventoryManager` - Handles product storage and dispensing
- `PaymentProcessor` - Manages money and change
- `Transaction` - Represents purchase records

```csharp
public class VendingMachine
{
    private readonly InventoryManager _inventoryManager;
    private readonly PaymentProcessor _paymentProcessor;
    
    // Simple interface hiding complexity
    public void InsertMoney(decimal amount) => _paymentProcessor.AddBalance(amount);
    public void ChooseProduct(string rackId) { /* Uses inventory manager */ }
    public Transaction ConfirmTransaction() { /* Orchestrates all subsystems */ }
}
```

### 2. Single Responsibility Principle (SRP)

Each class has one clear responsibility:
- `Product` - Represents a purchasable item with code, description, and price
- `Rack` - Manages inventory for a specific slot
- `InventoryManager` - Coordinates all racks
- `PaymentProcessor` - Handles money operations
- `Transaction` - Records completed purchases
- `VendingMachine` - Orchestrates the purchase flow

### 3. Encapsulation

Internal state is protected through proper access modifiers:
- Payment balance is managed only through `AddBalance()`, `Charge()`, and `ReturnChange()`
- Rack count changes only through `SetCount()` and `DispenseProductFromRack()`
- Transaction history is exposed as `IReadOnlyList<Transaction>`

## Key Classes

### VendingMachine

The main orchestrator that manages the purchase flow:

```csharp
public Transaction ConfirmTransaction()
{
    // Step 1: Validate the transaction
    ValidateTransaction();
    
    // Step 2: Charge the customer
    _paymentProcessor.Charge(_currentTransaction.Product!.UnitPrice);
    
    // Step 3: Dispense the product
    _inventoryManager.DispenseProductFromRack(_currentTransaction.Rack!);
    
    // Step 4: Return change
    _currentTransaction.TotalAmount = _paymentProcessor.ReturnChange();
    
    // Step 5: Record transaction
    _transactionHistory.Add(_currentTransaction);
    
    return _currentTransaction;
}
```

### PaymentProcessor

Handles all money-related operations:

```csharp
public class PaymentProcessor
{
    private decimal _currentBalance;
    
    public void AddBalance(decimal amount) => _currentBalance += amount;
    public void Charge(decimal amount) => _currentBalance -= amount;
    public decimal ReturnChange()
    {
        decimal change = _currentBalance;
        _currentBalance = 0;
        return change;
    }
}
```

### InventoryManager

Manages product racks:

```csharp
public class InventoryManager
{
    private Dictionary<string, Rack> _racks;
    
    public Product GetProductInRack(string rackCode) => _racks[rackCode].Product;
    public void DispenseProductFromRack(Rack rack) => rack.SetCount(rack.ProductCount - 1);
}
```

## Transaction Flow

```
┌──────────────────┐
│  Create Machine  │
└────────┬─────────┘
         ▼
┌──────────────────┐
│   SetRack()      │  Configure products
└────────┬─────────┘
         ▼
┌──────────────────┐
│  InsertMoney()   │  Customer inserts money
└────────┬─────────┘
         ▼
┌──────────────────┐
│ ChooseProduct()  │  Customer selects rack
└────────┬─────────┘
         ▼
    ┌────────────┐
    │ Confirm or │
    │  Cancel?   │
    └─────┬──────┘
          │
    ┌─────┴─────┐
    ▼           ▼
┌────────┐  ┌────────────┐
│Cancel  │  │  Confirm   │
└────┬───┘  └─────┬──────┘
     │            ▼
     │      ┌────────────┐
     │      │ Validate   │──Error──► InvalidTransactionException
     │      └─────┬──────┘
     │            ▼
     │      ┌────────────┐
     │      │  Charge    │
     │      └─────┬──────┘
     │            ▼
     │      ┌────────────┐
     │      │  Dispense  │
     │      └─────┬──────┘
     │            ▼
     │      ┌────────────┐
     │      │Return Change│
     │      └─────┬──────┘
     │            │
     └────────────┴──────► Ready for next transaction
```

## Usage Example

```csharp
// Initialize vending machine
var machine = new VendingMachine();

// Configure products
var cola = new Product("cola", "Coca Cola", 1.50m);
var chips = new Product("chips", "Potato Chips", 1.25m);
var candy = new Product("candy", "Chocolate Bar", 1.00m);

machine.SetRack(new Dictionary<string, Rack>
{
    { "A1", new Rack("A1", cola, 10) },
    { "A2", new Rack("A2", chips, 15) },
    { "B1", new Rack("B1", candy, 20) }
});

// Customer makes a purchase
machine.InsertMoney(5.00m);
machine.ChooseProduct("A1");

try
{
    var transaction = machine.ConfirmTransaction();
    Console.WriteLine($"Purchased: {transaction.Product?.Description}");
    Console.WriteLine($"Change: {transaction.TotalAmount:C}");
}
catch (InvalidTransactionException ex)
{
    Console.WriteLine($"Transaction failed: {ex.Message}");
    machine.CancelTransaction();
}

// Check inventory
var inventory = machine.GetInventoryManager();
Console.WriteLine($"Cola remaining: {inventory.GetRack("A1").ProductCount}");

// View transaction history
foreach (var tx in machine.GetTransactionHistory())
{
    Console.WriteLine(tx);
}
```

## Error Handling

The system validates transactions and throws `InvalidTransactionException` for:
- **Invalid product selection** - No product was selected before confirming
- **Insufficient inventory** - Selected product is out of stock
- **Insufficient fund** - Inserted money is less than product price

## Project Structure

```
OodInterview.VendingMachine/
├── VendingMachine.cs             # Main orchestrator (Facade)
├── Product.cs                    # Product representation
├── Rack.cs                       # Product slot with inventory
├── Transaction.cs                # Completed purchase record
├── InventoryManager.cs           # Rack management
├── PaymentProcessor.cs           # Money handling
└── InvalidTransactionException.cs # Custom exception
```

## Testing

Run the tests with:

```bash
dotnet test tests/OodInterview.VendingMachine.Tests
```

The test suite covers:
- End-to-end vending operations
- Insufficient funds validation
- Transaction cancellation
- No product selected validation
- Out of stock validation
- Multiple purchases with different products
- Individual component tests (Product, Rack, PaymentProcessor, InventoryManager, Transaction)
