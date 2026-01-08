namespace OodInterview.Blackjack;

/// <summary>
/// Represents a real player in the Blackjack game.
/// </summary>
public class RealPlayer : IPlayer
{
    private readonly Hand _hand;
    private int _bet;
    private int _balance;

    /// <summary>
    /// Constructor for RealPlayer.
    /// </summary>
    public RealPlayer(string name, int startBalance)
    {
        Name = name;
        _hand = new Hand();
        _bet = 0;
        _balance = startBalance;
    }

    /// <summary>
    /// Places a bet for the player.
    /// </summary>
    public void PlaceBet(int bet)
    {
        if (bet > _balance)
        {
            throw new ArgumentException("Bet is greater than balance");
        }
        _bet = bet;
        _balance -= bet;
    }

    /// <summary>
    /// Handles the player losing a bet.
    /// </summary>
    public void LoseBet()
    {
        _bet = 0;
    }

    /// <summary>
    /// Handles returning the player's bet (tie scenario).
    /// </summary>
    public void ReturnBet()
    {
        _balance += _bet;
        _bet = 0;
    }

    /// <summary>
    /// Handles the player winning a payout.
    /// </summary>
    public void Payout()
    {
        _balance += _bet * 2; // Return bet plus equal amount
        _bet = 0;
    }

    /// <summary>
    /// Checks if the player is bust.
    /// </summary>
    public bool IsBust => _hand.IsBust;

    /// <summary>
    /// Returns the player's hand.
    /// </summary>
    public Hand Hand => _hand;

    /// <summary>
    /// Returns the player's balance.
    /// </summary>
    public int Balance => _balance;

    /// <summary>
    /// Returns the player's name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns the player's current bet.
    /// </summary>
    public int CurrentBet => _bet;
}
