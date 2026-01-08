namespace OodInterview.Blackjack;

/// <summary>
/// Represents a playing card with a rank and suit.
/// </summary>
public class Card
{
    public Rank Rank { get; }
    public Suit Suit { get; }

    /// <summary>
    /// Constructor for Card.
    /// </summary>
    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    /// <summary>
    /// Returns the possible values for the card's rank.
    /// </summary>
    public int[] GetRankValues() => Rank.GetRankValues();

    /// <summary>
    /// Returns a string representation of the card.
    /// </summary>
    public override string ToString() => $"Card{{rank={Rank}, suit={Suit}}}";
}
