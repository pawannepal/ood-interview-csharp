namespace OodInterview.TicTacToe;

/// <summary>
/// Represents a player in the game.
/// </summary>
public class Player
{
    /// <summary>
    /// Creates a new player with a name and symbol.
    /// </summary>
    /// <param name="name">Player's name identifier.</param>
    /// <param name="symbol">Player's symbol (X or O).</param>
    public Player(string name, char symbol)
    {
        Name = name;
        Symbol = symbol;
    }

    /// <summary>
    /// Gets the player's name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the player's symbol (X or O).
    /// </summary>
    public char Symbol { get; }

    /// <summary>
    /// Returns a string representation of the player (their name).
    /// </summary>
    public override string ToString() => Name;
}
