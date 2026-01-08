namespace OodInterview.Blackjack;

/// <summary>
/// Represents a standard 52-card deck.
/// </summary>
public class Deck
{
    internal int NextCardIndex { get; set; } = 0;
    internal List<Card> Cards { get; set; } = [];

    /// <summary>
    /// Constructor initializes the deck.
    /// </summary>
    public Deck()
    {
        InitializeDeck();
    }

    /// <summary>
    /// Initializes the deck with all 52 cards.
    /// </summary>
    private void InitializeDeck()
    {
        Cards = [];
        foreach (Suit suit in Enum.GetValues<Suit>())
        {
            foreach (Rank rank in Enum.GetValues<Rank>())
            {
                Cards.Add(new Card(rank, suit));
            }
        }
        NextCardIndex = 0;
    }

    /// <summary>
    /// Shuffles the deck using current time as seed.
    /// </summary>
    public void Shuffle()
    {
        Shuffle(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    /// <summary>
    /// Shuffles the deck using a provided seed.
    /// </summary>
    public void Shuffle(long seed)
    {
        var random = new Random((int)seed);
        int n = Cards.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (Cards[i], Cards[j]) = (Cards[j], Cards[i]);
        }
    }

    /// <summary>
    /// Draws the next card from the deck.
    /// </summary>
    public Card? Draw()
    {
        if (IsEmpty) return null;
        var drawCard = Cards[NextCardIndex];
        NextCardIndex++;
        return drawCard;
    }

    /// <summary>
    /// Returns the number of remaining cards in the deck.
    /// </summary>
    public int RemainingCardCount => Cards.Count - NextCardIndex;

    /// <summary>
    /// Checks if the deck is empty.
    /// </summary>
    public bool IsEmpty => RemainingCardCount == 0;

    /// <summary>
    /// Resets the deck to start drawing from the beginning.
    /// </summary>
    public void Reset()
    {
        NextCardIndex = 0;
    }
}
