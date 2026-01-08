# ATM System

A .NET Core 10 implementation of an ATM (Automated Teller Machine) system that simulates basic banking operations like deposits, withdrawals, and PIN validation.

## Design Patterns Used

### State Pattern

The ATM system is a classic example of the **State Pattern**. The ATM behavior changes based on its current state, and each state handles user actions differently.

#### State Diagram

```
┌─────────────┐
│  IdleState  │ ◄────────────────────────────────────┐
└──────┬──────┘                                       │
       │ Insert Valid Card                            │
       ▼                                              │
┌──────────────────┐                                  │
│  PinEntryState   │                                  │
└────────┬─────────┘                                  │
         │ Enter Valid PIN                            │
         ▼                                            │
┌───────────────────────────┐                         │
│ TransactionSelectionState │ ◄──────┐               │
└───────────┬───────────────┘        │               │
            │                        │               │
   ┌────────┴────────┐               │               │
   │                 │               │               │
   ▼                 ▼               │               │
┌──────────────┐  ┌─────────────────┐│               │
│ Withdraw     │  │ DepositCollection││               │
│ AmountEntry  │  │     State       ││               │
│   State      │  └────────┬────────┘│               │
└──────┬───────┘           │         │               │
       │                   │         │               │
       └───────────────────┴─────────┘               │
               │                                     │
               │ Eject Card (from any state)         │
               └─────────────────────────────────────┘
```

#### States

| State | Description | Valid Actions |
|-------|-------------|---------------|
| `IdleState` | Waiting for a card to be inserted | Card Insertion |
| `PinEntryState` | Waiting for PIN entry | PIN Entry, Card Ejection |
| `TransactionSelectionState` | Waiting for transaction type selection | Withdraw Request, Deposit Request, Card Ejection |
| `WithdrawAmountEntryState` | Waiting for withdrawal amount | Amount Entry, Card Ejection |
| `DepositCollectionState` | Waiting for cash deposit | Deposit Collection, Card Ejection |

### Why State Pattern?

1. **Encapsulation**: Each state encapsulates its own behavior, making the code easier to understand and maintain.
2. **Open/Closed Principle**: Adding new states doesn't require modifying existing state classes.
3. **Single Responsibility**: Each state class has one job - handling actions for that specific state.
4. **Eliminates Complex Conditionals**: Instead of large switch/if-else blocks, behavior is delegated to state objects.

## Project Structure

```
OodInterview.Atm/
├── AtmMachine.cs              # Main ATM machine class (Context)
├── Bank/
│   ├── Account.cs             # Bank account with PIN security
│   ├── Bank.cs                # Bank implementation
│   ├── IBankInterface.cs      # Bank operations contract
│   ├── ITransaction.cs        # Transaction interface
│   ├── DepositTransaction.cs  # Deposit transaction implementation
│   ├── WithdrawTransaction.cs # Withdrawal transaction implementation
│   └── Enums/
│       ├── AccountType.cs     # Account types (Checking, Saving)
│       └── TransactionType.cs # Transaction types (Withdraw, Deposit)
├── Hardware/
│   ├── Input/
│   │   ├── ICardProcessor.cs      # Card processor interface
│   │   ├── IDepositBox.cs         # Deposit box interface
│   │   ├── IKeypad.cs             # Keypad interface
│   │   ├── SimulatedCardProcessor.cs
│   │   ├── SimulatedDepositBox.cs
│   │   └── SimulatedKeypad.cs
│   └── Output/
│       ├── ICashDispenser.cs      # Cash dispenser interface
│       ├── IDisplay.cs            # Display interface
│       ├── ConsoleDisplay.cs
│       └── SimulatedCashDispenser.cs
└── States/
    ├── AtmState.cs               # Base state class
    ├── IdleState.cs              # Waiting for card
    ├── PinEntryState.cs          # Waiting for PIN
    ├── TransactionSelectionState.cs
    ├── WithdrawAmountEntryState.cs
    └── DepositCollectionState.cs
```

## Key Components

### AtmMachine (Context)

The `AtmMachine` class is the **Context** in the State Pattern. It:
- Maintains a reference to the current state
- Delegates user actions to the current state
- Provides access to hardware components and bank interface

```csharp
public class AtmMachine
{
    private AtmState _state;
    
    // Hardware components
    private readonly ICardProcessor _cardProcessor;
    private readonly IDepositBox _depositBox;
    private readonly ICashDispenser _cashDispenser;
    private readonly IKeypad _keypad;
    private readonly IDisplay _display;
    private readonly IBankInterface _bank;
    
    // User actions are delegated to the current state
    public void InsertCard(string cardNumber) => _state.ProcessCardInsertion(this, cardNumber);
    public void EnterPin(string pin) => _state.ProcessPinEntry(this, pin);
    public void WithdrawRequest() => _state.ProcessWithdrawalRequest(this);
    public void DepositRequest() => _state.ProcessDepositRequest(this);
    
    // State transition
    public void TransitionToState(AtmState nextState) => _state = nextState;
}
```

### AtmState (State Interface)

The `AtmState` class is the base class for all states. It provides default implementations that show an error message. Concrete states override methods they care about.

```csharp
public class AtmState
{
    // Default implementation - shows error
    private static void RenderDefaultAction(AtmMachine atmMachine)
    {
        atmMachine.Display.ShowMessage("Invalid action, please try again.");
    }

    public virtual void ProcessCardInsertion(AtmMachine atmMachine, string cardNumber)
        => RenderDefaultAction(atmMachine);
    
    public virtual void ProcessPinEntry(AtmMachine atmMachine, string pin)
        => RenderDefaultAction(atmMachine);
    
    // ... other methods
}
```

### Hardware Abstraction

The ATM hardware is abstracted behind interfaces, allowing:
- Easy testing with simulated components
- Potential for real hardware integration
- Separation of concerns

## Security Features

- **PIN Hashing**: PINs are stored as MD5 hashes, never in plain text
- **Card Validation**: Cards are validated against the bank's records before allowing transactions

## Running the Tests

```bash
# From repository root
dotnet test tests/OodInterview.Atm.Tests

# With detailed output
dotnet test tests/OodInterview.Atm.Tests --logger "console;verbosity=detailed"
```

## Test Cases

| Test | Description |
|------|-------------|
| `TestEndToEndDeposit` | Complete deposit transaction flow |
| `TestMultipleDeposits` | Multiple deposits in one session |
| `TestMultipleWithdrawals` | Multiple withdrawals with balance checks |
| `TestInvalidCardInsertion` | Handling of invalid/unknown cards |
| `TestInvalidPinEntry` | Handling of incorrect PIN entry |
| `TestWithdrawWithoutSufficientFunds` | Insufficient funds handling |
| `TestEjectCardDuringTransaction` | Card ejection cancels transaction |

## Example Usage

```csharp
// Create a bank with an account
var bank = new Bank();
bank.AddAccount("123456", AccountType.Saving, "1111-2222-3333-4444", "1234");
bank.GetAccountByAccountNumber("123456")!.UpdateBalanceWithTransaction(500m);

// Create ATM with simulated hardware
var atmMachine = new AtmMachine(
    bank,
    new SimulatedCardProcessor(),
    new SimulatedDepositBox(),
    new SimulatedCashDispenser(),
    new SimulatedKeypad(),
    new ConsoleDisplay()
);

// Use the ATM
cardProcessor.HandleCardInsertion("1111-2222-3333-4444", atmMachine);
keypad.HandlePinEntry("1234", atmMachine);
keypad.HandleSelectTransaction(TransactionType.Withdraw, atmMachine);
keypad.HandleAmountEntry(100m, atmMachine);
// Cash is dispensed, balance is now 400
```

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `BigDecimal` | `decimal` |
| `MessageDigest.getInstance("MD5")` | `MD5.Create()` |
| `Arrays.equals()` | `SequenceEqual()` |
| `MessageFormat.format()` | String interpolation `$""` |
| Package-private access | `internal` access modifier |
| `Optional<T>` | Nullable reference types `T?` |
