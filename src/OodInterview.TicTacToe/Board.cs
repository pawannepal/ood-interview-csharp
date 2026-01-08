namespace OodInterview.TicTacToe;

/// <summary>
/// Manages the 3x3 game board.
/// </summary>
public class Board
{
    private readonly Player?[,] _grid = new Player?[3, 3];

    /// <summary>
    /// Updates the board with a player's move at the specified position.
    /// </summary>
    public void UpdateBoard(int colIndex, int rowIndex, Player? player)
    {
        _grid[colIndex, rowIndex] = player;
    }

    /// <summary>
    /// Returns the player at the specified position, or null if empty.
    /// </summary>
    public Player? GetPlayerAt(int colIndex, int rowIndex)
    {
        return _grid[colIndex, rowIndex];
    }

    /// <summary>
    /// Checks for a winner by examining rows, columns, and diagonals.
    /// </summary>
    public Player? GetWinner()
    {
        // Check rows for three in a row
        for (var i = 0; i < 3; i++)
        {
            var first = _grid[i, 0];
            if (first != null && _grid[i, 1] == first && _grid[i, 2] == first)
            {
                return first;
            }
        }

        // Check columns for three in a column
        for (var j = 0; j < 3; j++)
        {
            var first = _grid[0, j];
            if (first != null && _grid[1, j] == first && _grid[2, j] == first)
            {
                return first;
            }
        }

        // Check main diagonal (top-left to bottom-right)
        var topLeft = _grid[0, 0];
        if (topLeft != null && _grid[1, 1] == topLeft && _grid[2, 2] == topLeft)
        {
            return topLeft;
        }

        // Check anti-diagonal (top-right to bottom-left)
        var topRight = _grid[0, 2];
        if (topRight != null && _grid[1, 1] == topRight && _grid[2, 0] == topRight)
        {
            return topRight;
        }

        // No winner found
        return null;
    }

    /// <summary>
    /// Checks if all positions on the board are filled.
    /// </summary>
    public bool IsFull()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (_grid[i, j] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Resets the board by clearing all positions.
    /// </summary>
    public void Reset()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                _grid[i, j] = null;
            }
        }
    }

    /// <summary>
    /// Returns a string representation of the current board state.
    /// </summary>
    public string PrintBoard()
    {
        var lines = new List<string>();
        for (var i = 0; i < 3; i++)
        {
            var row = new List<string>();
            for (var j = 0; j < 3; j++)
            {
                var player = _grid[i, j];
                row.Add(player?.Symbol.ToString() ?? "_");
            }
            lines.Add(string.Join(" ", row));
        }
        return string.Join("\n", lines);
    }
}
