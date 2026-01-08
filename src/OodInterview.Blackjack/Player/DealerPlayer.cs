namespace OodInterview.Blackjack;

/// <summary>
/// Represents the dealer in the Blackjack game.
/// </summary>
public class DealerPlayer : IPlayer
{
    private readonly Hand _hand;

    /// <summary>
    /// Constructor for DealerPlayer.
    /// </summary>
    public DealerPlayer()
    {
        _hand = new Hand();
    }

    /// <summary>
    /// Dealer does not bet, so this method is empty.
    /// </summary>
    public void PlaceBet(int bet)
    {
        // Dealer does not bet
    }

    /// <summary>
    /// Dealer does not lose bets, so this method is empty.
    /// </summary>
    public void LoseBet()
    {
        // Dealer does not lose bets
    }

    /// <summary>
    /// Dealer does not return bets, so this method is empty.
    /// </summary>
    public void ReturnBet()
    {
        // Dealer does not return bets
    }

    /// <summary>
    /// Dealer does not get a payout, so this method is empty.
    /// </summary>
    public void Payout()
    {
        // Player won with card value
    }

    /// <summary>
    /// Checks if the dealer is bust.
    /// </summary>
    public bool IsBust => _hand.IsBust;

    /// <summary>
    /// Returns the dealer's hand.
    /// </summary>
    public Hand Hand => _hand;

    /// <summary>
    /// Returns the dealer's balance (always 0).
    /// </summary>
    public int Balance => 0;

    /// <summary>
    /// Returns the dealer's name.
    /// </summary>
    public string Name => "Dealer";

    /// <summary>
    /// Returns the dealer's bet (always 0).
    /// </summary>
    public int CurrentBet => 0;
}
