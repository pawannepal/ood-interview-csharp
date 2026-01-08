using OodInterview.Atm;
using OodInterview.Atm.Bank;
using OodInterview.Atm.Bank.Enums;
using OodInterview.Atm.Hardware.Input;
using OodInterview.Atm.Hardware.Output;
using OodInterview.Atm.States;

namespace OodInterview.Atm.Tests;

/// <summary>
/// Test suite for ATM machine functionality including deposits, withdrawals, and error handling.
/// </summary>
public class AtmMachineTests
{
    [Fact]
    public void TestEndToEndDeposit()
    {
        // Initialize bank
        var bank = new Bank.Bank();
        bank.AddAccount("123456", AccountType.Saving, "1111-2222-3333-4444", "1234");

        // Initialize simulated ATM components
        var cardProcessor = new SimulatedCardProcessor();
        var depositBox = new SimulatedDepositBox();
        var display = new ConsoleDisplay();
        var keypad = new SimulatedKeypad();
        var cashDispenser = new SimulatedCashDispenser();

        var atmMachine = new AtmMachine(bank, cardProcessor, depositBox, cashDispenser, keypad, display);

        // First check idle state
        Assert.IsType<IdleState>(atmMachine.CurrentState);

        // Step 1: Insert card
        cardProcessor.HandleCardInsertion("1111-2222-3333-4444", atmMachine);
        Assert.Equal("Please enter your PIN", display.DisplayedMessage);

        // Step 2: Enter PIN
        keypad.HandlePinEntry("1234", atmMachine);
        Assert.Equal("PIN correct, select transaction type", display.DisplayedMessage);

        // Step 3: Select deposit
        keypad.HandleSelectTransaction(TransactionType.Deposit, atmMachine);
        Assert.Equal("Please deposit cash into the deposit box.", display.DisplayedMessage);

        // Step 4: Simulate deposit
        depositBox.AcceptDeposit(200m, atmMachine);
        Assert.Equal("Deposit successful. Deposited amount: 200 to account: 123456", display.DisplayedMessage);

        // Check if the ATM is in the transaction selection state
        Assert.IsType<TransactionSelectionState>(atmMachine.CurrentState);

        // Step 5: Verify account
        Assert.Equal(200m, bank.GetAccountByAccountNumber("123456")!.Balance);
    }

    [Fact]
    public void TestMultipleDeposits()
    {
        // Initialize bank
        var bank = new Bank.Bank();
        bank.AddAccount("123456", AccountType.Saving, "1111-2222-3333-4444", "1234");

        // Initialize simulated ATM components
        var cardProcessor = new SimulatedCardProcessor();
        var depositBox = new SimulatedDepositBox();
        var display = new ConsoleDisplay();
        var keypad = new SimulatedKeypad();
        var cashDispenser = new SimulatedCashDispenser();

        var atmMachine = new AtmMachine(bank, cardProcessor, depositBox, cashDispenser, keypad, display);

        // First check idle state
        Assert.IsType<IdleState>(atmMachine.CurrentState);

        // Step 1: Insert card
        cardProcessor.HandleCardInsertion("1111-2222-3333-4444", atmMachine);
        Assert.Equal("Please enter your PIN", display.DisplayedMessage);

        // Step 2: Enter PIN
        keypad.HandlePinEntry("1234", atmMachine);
        Assert.Equal("PIN correct, select transaction type", display.DisplayedMessage);

        // Step 3: Select deposit
        keypad.HandleSelectTransaction(TransactionType.Deposit, atmMachine);
        Assert.Equal("Please deposit cash into the deposit box.", display.DisplayedMessage);

        // Step 4: Simulate first deposit
        depositBox.AcceptDeposit(200m, atmMachine);
        Assert.Equal("Deposit successful. Deposited amount: 200 to account: 123456", display.DisplayedMessage);

        // Step 5: Verify account balance after first deposit
        Assert.Equal(200m, bank.GetAccountByAccountNumber("123456")!.Balance);

        // Step 6: From transaction selection, select deposit again
        keypad.HandleSelectTransaction(TransactionType.Deposit, atmMachine);
        Assert.Equal("Please deposit cash into the deposit box.", display.DisplayedMessage);

        // Step 7: Simulate second deposit
        depositBox.AcceptDeposit(300m, atmMachine);
        Assert.Equal("Deposit successful. Deposited amount: 300 to account: 123456", display.DisplayedMessage);

        // Step 8: Verify account balance after second deposit
        Assert.Equal(500m, bank.GetAccountByAccountNumber("123456")!.Balance);
    }

    [Fact]
    public void TestMultipleWithdrawals()
    {
        // Initialize bank
        var bank = new Bank.Bank();
        bank.AddAccount("123456", AccountType.Saving, "1111-2222-3333-4444", "1234");
        bank.GetAccountByAccountNumber("123456")!.UpdateBalanceWithTransaction(500m);

        // Initialize simulated ATM components
        var cardProcessor = new SimulatedCardProcessor();
        var depositBox = new SimulatedDepositBox();
        var display = new ConsoleDisplay();
        var keypad = new SimulatedKeypad();
        var cashDispenser = new SimulatedCashDispenser();

        var atmMachine = new AtmMachine(bank, cardProcessor, depositBox, cashDispenser, keypad, display);

        // First check idle state
        Assert.IsType<IdleState>(atmMachine.CurrentState);

        // Step 1: Insert card
        cardProcessor.HandleCardInsertion("1111-2222-3333-4444", atmMachine);
        Assert.Equal("Please enter your PIN", display.DisplayedMessage);

        // Step 2: Enter PIN
        keypad.HandlePinEntry("1234", atmMachine);
        Assert.Equal("PIN correct, select transaction type", display.DisplayedMessage);

        // Step 3: Select withdraw
        keypad.HandleSelectTransaction(TransactionType.Withdraw, atmMachine);
        Assert.Equal("Enter amount to withdraw:", display.DisplayedMessage);

        // Step 4: Simulate first withdrawal
        keypad.HandleAmountEntry(200m, atmMachine);
        Assert.Equal("Please take your cash.", display.DisplayedMessage);

        // Step 5: Verify account balance after first withdrawal
        Assert.Equal(300m, bank.GetAccountByAccountNumber("123456")!.Balance);

        // Step 6: Select withdraw again
        keypad.HandleSelectTransaction(TransactionType.Withdraw, atmMachine);
        Assert.Equal("Enter amount to withdraw:", display.DisplayedMessage);

        // Step 7: Simulate second withdrawal
        keypad.HandleAmountEntry(100m, atmMachine);
        Assert.Equal("Please take your cash.", display.DisplayedMessage);

        // Step 8: Verify account balance after second withdrawal
        Assert.Equal(200m, bank.GetAccountByAccountNumber("123456")!.Balance);

        // Step 9: Attempt withdrawal exceeding balance
        keypad.HandleSelectTransaction(TransactionType.Withdraw, atmMachine);
        keypad.HandleAmountEntry(500m, atmMachine);
        Assert.Equal("Insufficient funds, please try again.", display.DisplayedMessage);

        // Step 10: Verify balance unchanged after failed withdrawal
        Assert.Equal(200m, bank.GetAccountByAccountNumber("123456")!.Balance);
    }

    [Fact]
    public void TestInvalidCardInsertion()
    {
        // Initialize bank (no accounts registered)
        var bank = new Bank.Bank();

        // Initialize simulated ATM components
        var cardProcessor = new SimulatedCardProcessor();
        var depositBox = new SimulatedDepositBox();
        var display = new ConsoleDisplay();
        var keypad = new SimulatedKeypad();
        var cashDispenser = new SimulatedCashDispenser();

        var atmMachine = new AtmMachine(bank, cardProcessor, depositBox, cashDispenser, keypad, display);

        // First check idle state
        Assert.IsType<IdleState>(atmMachine.CurrentState);

        // Attempt to insert an invalid card
        cardProcessor.HandleCardInsertion("9999-8888-7777-6666", atmMachine);
        Assert.Equal("Invalid card. Please try again.", display.DisplayedMessage);

        // Verify state remains IdleState
        Assert.IsType<IdleState>(atmMachine.CurrentState);
    }

    [Fact]
    public void TestInvalidPinEntry()
    {
        // Initialize bank
        var bank = new Bank.Bank();
        bank.AddAccount("123456", AccountType.Saving, "1111-2222-3333-4444", "1234");

        // Initialize simulated ATM components
        var cardProcessor = new SimulatedCardProcessor();
        var depositBox = new SimulatedDepositBox();
        var display = new ConsoleDisplay();
        var keypad = new SimulatedKeypad();
        var cashDispenser = new SimulatedCashDispenser();

        var atmMachine = new AtmMachine(bank, cardProcessor, depositBox, cashDispenser, keypad, display);

        // Insert valid card
        cardProcessor.HandleCardInsertion("1111-2222-3333-4444", atmMachine);
        Assert.Equal("Please enter your PIN", display.DisplayedMessage);

        // Enter invalid PIN
        keypad.HandlePinEntry("0000", atmMachine);
        Assert.Equal("Invalid PIN. Please try again", display.DisplayedMessage);

        // Verify state remains PinEntryState
        Assert.IsType<PinEntryState>(atmMachine.CurrentState);
    }

    [Fact]
    public void TestWithdrawWithoutSufficientFunds()
    {
        // Initialize bank
        var bank = new Bank.Bank();
        bank.AddAccount("123456", AccountType.Saving, "1111-2222-3333-4444", "1234");
        bank.GetAccountByAccountNumber("123456")!.UpdateBalanceWithTransaction(100m);

        // Initialize simulated ATM components
        var cardProcessor = new SimulatedCardProcessor();
        var depositBox = new SimulatedDepositBox();
        var display = new ConsoleDisplay();
        var keypad = new SimulatedKeypad();
        var cashDispenser = new SimulatedCashDispenser();

        var atmMachine = new AtmMachine(bank, cardProcessor, depositBox, cashDispenser, keypad, display);

        // Insert valid card
        cardProcessor.HandleCardInsertion("1111-2222-3333-4444", atmMachine);
        Assert.Equal("Please enter your PIN", display.DisplayedMessage);

        // Enter valid PIN
        keypad.HandlePinEntry("1234", atmMachine);
        Assert.Equal("PIN correct, select transaction type", display.DisplayedMessage);

        // Select withdraw
        keypad.HandleSelectTransaction(TransactionType.Withdraw, atmMachine);
        Assert.Equal("Enter amount to withdraw:", display.DisplayedMessage);

        // Attempt to withdraw more than balance
        keypad.HandleAmountEntry(200m, atmMachine);
        Assert.Equal("Insufficient funds, please try again.", display.DisplayedMessage);

        // Verify state remains TransactionSelectionState
        Assert.IsType<TransactionSelectionState>(atmMachine.CurrentState);
    }

    [Fact]
    public void TestEjectCardDuringTransaction()
    {
        // Initialize bank
        var bank = new Bank.Bank();
        bank.AddAccount("123456", AccountType.Saving, "1111-2222-3333-4444", "1234");

        // Initialize simulated ATM components
        var cardProcessor = new SimulatedCardProcessor();
        var depositBox = new SimulatedDepositBox();
        var display = new ConsoleDisplay();
        var keypad = new SimulatedKeypad();
        var cashDispenser = new SimulatedCashDispenser();

        var atmMachine = new AtmMachine(bank, cardProcessor, depositBox, cashDispenser, keypad, display);

        // Insert valid card
        cardProcessor.HandleCardInsertion("1111-2222-3333-4444", atmMachine);
        Assert.Equal("Please enter your PIN", display.DisplayedMessage);

        // Enter valid PIN
        keypad.HandlePinEntry("1234", atmMachine);
        Assert.Equal("PIN correct, select transaction type", display.DisplayedMessage);

        // Select deposit
        keypad.HandleSelectTransaction(TransactionType.Deposit, atmMachine);
        Assert.Equal("Please deposit cash into the deposit box.", display.DisplayedMessage);

        // Eject card during transaction
        cardProcessor.HandleCardEjection(atmMachine);
        Assert.Equal("Transaction cancelled, card ejected", display.DisplayedMessage);

        // Verify state transitions back to IdleState
        Assert.IsType<IdleState>(atmMachine.CurrentState);
    }
}
