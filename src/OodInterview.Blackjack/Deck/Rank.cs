namespace OodInterview.Blackjack;

/// <summary>
/// Enum representing the rank of a playing card.
/// </summary>
public enum Rank
{
    Ace,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

/// <summary>
/// Extension methods for the Rank enum.
/// </summary>
public static class RankExtensions
{
    /// <summary>
    /// Returns the possible values for the rank.
    /// Aces can be 1 or 11, face cards are 10.
    /// </summary>
    public static int[] GetRankValues(this Rank rank) => rank switch
    {
        Rank.Ace => [1, 11],
        Rank.Two => [2],
        Rank.Three => [3],
        Rank.Four => [4],
        Rank.Five => [5],
        Rank.Six => [6],
        Rank.Seven => [7],
        Rank.Eight => [8],
        Rank.Nine => [9],
        Rank.Ten => [10],
        Rank.Jack => [10],
        Rank.Queen => [10],
        Rank.King => [10],
        _ => throw new ArgumentOutOfRangeException(nameof(rank))
    };
}
