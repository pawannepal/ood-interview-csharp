namespace OodInterview.Blackjack;

/// <summary>
/// Represents a player's hand in Blackjack.
/// </summary>
public class Hand
{
    private readonly List<Card> _handCards = [];
    
    /// <summary>
    /// Sorted set of all possible hand values, accounting for Ace flexibility (1 or 11).
    /// </summary>
    private readonly SortedSet<int> _possibleValues = [];

    /// <summary>
    /// Adds a card to the hand and updates the set of possible total values.
    /// For Aces (1 or 11), computes all combinations with existing totals; 
    /// for other cards, adds their value to each total.
    /// </summary>
    public void AddCard(Card card)
    {
        ArgumentNullException.ThrowIfNull(card);
        
        _handCards.Add(card);

        // card.GetRankValues() returns [1, 11] for Aces or a single value (e.g., [10]) for others.
        if (_possibleValues.Count == 0)
        {
            // Initialize with the card's values
            foreach (int value in card.GetRankValues())
            {
                _possibleValues.Add(value);
            }
        }
        else
        {
            // Add all possible card values to each existing total
            var newPossibleValues = new SortedSet<int>();
            foreach (int value in _possibleValues)
            {
                foreach (int cardValue in card.GetRankValues())
                {
                    newPossibleValues.Add(value + cardValue);
                }
            }
            _possibleValues.Clear();
            foreach (int value in newPossibleValues)
            {
                _possibleValues.Add(value);
            }
        }
    }

    /// <summary>
    /// Returns an unmodifiable list of cards in the hand.
    /// </summary>
    public IReadOnlyList<Card> Cards => _handCards.AsReadOnly();

    /// <summary>
    /// Returns an unmodifiable sorted set of possible hand values.
    /// </summary>
    public IReadOnlyCollection<int> PossibleValues => _possibleValues;

    /// <summary>
    /// Returns the maximum possible value without busting, or the minimum if all bust.
    /// </summary>
    public int BestValue
    {
        get
        {
            if (_possibleValues.Count == 0) return 0;
            
            // Find the highest value <= 21
            var validValues = _possibleValues.Where(v => v <= 21).ToList();
            if (validValues.Count > 0)
            {
                return validValues.Max();
            }
            
            // All values bust, return the minimum
            return _possibleValues.Min;
        }
    }

    /// <summary>
    /// Clears the hand and possible values.
    /// </summary>
    public void Clear()
    {
        _handCards.Clear();
        _possibleValues.Clear();
    }

    /// <summary>
    /// Checks if the hand is bust (all possible values > 21).
    /// </summary>
    public bool IsBust
    {
        get
        {
            if (_possibleValues.Count == 0) return false;
            return _possibleValues.Min > 21;
        }
    }

    /// <summary>
    /// Returns a string representation of the hand.
    /// </summary>
    public override string ToString() => 
        $"Hand{{handCards=[{string.Join(", ", _handCards)}], possibleValue=[{string.Join(", ", _possibleValues)}]}}";
}
