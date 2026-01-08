namespace OodInterview.Blackjack;

/// <summary>
/// Interface representing a player in Blackjack.
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Places a bet for the player.
    /// </summary>
    void PlaceBet(int bet);

    /// <summary>
    /// Handles the player losing a bet.
    /// </summary>
    void LoseBet();

    /// <summary>
    /// Handles returning the player's bet (tie scenario).
    /// </summary>
    void ReturnBet();

    /// <summary>
    /// Handles the player winning a payout.
    /// </summary>
    void Payout();

    /// <summary>
    /// Checks if the player is bust.
    /// </summary>
    bool IsBust { get; }

    /// <summary>
    /// Returns the player's hand.
    /// </summary>
    Hand Hand { get; }

    /// <summary>
    /// Returns the player's balance.
    /// </summary>
    int Balance { get; }

    /// <summary>
    /// Returns the player's name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns the player's current bet.
    /// </summary>
    int CurrentBet { get; }
}
