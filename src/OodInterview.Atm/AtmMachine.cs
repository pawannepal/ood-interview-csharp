using OodInterview.Atm.Bank;
using OodInterview.Atm.Hardware.Input;
using OodInterview.Atm.Hardware.Output;
using OodInterview.Atm.States;

namespace OodInterview.Atm;

/// <summary>
/// Main ATM machine class that manages the state and hardware components of the ATM.
/// </summary>
public class AtmMachine
{
    private AtmState _state;

    private readonly ICardProcessor _cardProcessor;
    private readonly IDepositBox _depositBox;
    private readonly ICashDispenser _cashDispenser;
    private readonly IKeypad _keypad;
    private readonly IDisplay _display;
    private readonly IBankInterface _bank;

    /// <summary>
    /// Initializes ATM with all required hardware components and bank interface.
    /// </summary>
    public AtmMachine(
        Bank.Bank bank,
        ICardProcessor cardProcessor,
        IDepositBox depositBox,
        ICashDispenser cashDispenser,
        IKeypad keypad,
        IDisplay display)
    {
        _bank = bank;
        _cardProcessor = cardProcessor;
        _depositBox = depositBox;
        _cashDispenser = cashDispenser;
        _keypad = keypad;
        _display = display;
        _state = new IdleState();
    }

    /// <summary>
    /// Forwards card insertion to current state for processing.
    /// </summary>
    public void InsertCard(string cardNumber)
    {
        _state.ProcessCardInsertion(this, cardNumber);
    }

    /// <summary>
    /// Forwards card ejection to current state for processing.
    /// </summary>
    public void EjectCard()
    {
        _state.ProcessCardEjection(this);
    }

    /// <summary>
    /// Forwards PIN entry to current state for validation.
    /// </summary>
    public void EnterPin(string pin)
    {
        _state.ProcessPinEntry(this, pin);
    }

    /// <summary>
    /// Forwards withdrawal request to current state for processing.
    /// </summary>
    public void WithdrawRequest()
    {
        _state.ProcessWithdrawalRequest(this);
    }

    /// <summary>
    /// Forwards deposit request to current state for processing.
    /// </summary>
    public void DepositRequest()
    {
        _state.ProcessDepositRequest(this);
    }

    /// <summary>
    /// Forwards amount entry to current state for processing.
    /// </summary>
    public void EnterAmount(decimal amount)
    {
        _state.ProcessAmountEntry(this, amount);
    }

    /// <summary>
    /// Forwards deposit collection to current state for processing.
    /// </summary>
    public void CollectDeposit(decimal amount)
    {
        _state.ProcessDepositCollection(this, amount);
    }

    /// <summary>
    /// Gets the display component for showing messages.
    /// </summary>
    public IDisplay Display => _display;

    /// <summary>
    /// Gets the cash dispenser component for handling withdrawals.
    /// </summary>
    public ICashDispenser CashDispenser => _cashDispenser;

    /// <summary>
    /// Gets the bank interface for account operations.
    /// </summary>
    public IBankInterface BankInterface => _bank;

    /// <summary>
    /// Gets the card processor component for handling card operations.
    /// </summary>
    public ICardProcessor CardProcessor => _cardProcessor;

    /// <summary>
    /// Gets the keypad component for user input.
    /// </summary>
    public IKeypad Keypad => _keypad;

    /// <summary>
    /// Gets the deposit box component for handling deposits.
    /// </summary>
    public IDepositBox DepositBox => _depositBox;

    /// <summary>
    /// Updates the current state of the ATM.
    /// </summary>
    public void TransitionToState(AtmState nextState)
    {
        _state = nextState;
    }

    /// <summary>
    /// Gets the current state of the ATM.
    /// </summary>
    public AtmState CurrentState => _state;
}
