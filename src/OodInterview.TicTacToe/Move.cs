namespace OodInterview.TicTacToe;

/// <summary>
/// Represents a move made by a player.
/// </summary>
public class Move
{
    /// <summary>
    /// Creates a new move with position and player information.
    /// </summary>
    public Move(int colIndex, int rowIndex, Player player)
    {
        ColIndex = colIndex;
        RowIndex = rowIndex;
        Player = player;
    }

    /// <summary>
    /// Gets the column index of the move (0-2).
    /// </summary>
    public int ColIndex { get; }

    /// <summary>
    /// Gets the row index of the move (0-2).
    /// </summary>
    public int RowIndex { get; }

    /// <summary>
    /// Gets the player who made the move.
    /// </summary>
    public Player Player { get; }
}
