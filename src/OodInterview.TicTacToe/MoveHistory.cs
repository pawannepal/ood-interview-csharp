namespace OodInterview.TicTacToe;

/// <summary>
/// Stack-like structure to store moves in chronological order.
/// </summary>
public class MoveHistory
{
    private readonly Stack<Move> _history = new();

    /// <summary>
    /// Adds a new move to the history stack.
    /// </summary>
    public void RecordMove(Move move)
    {
        _history.Push(move);
    }

    /// <summary>
    /// Removes and returns the most recent move from the history.
    /// </summary>
    public Move UndoMove()
    {
        return _history.Pop();
    }

    /// <summary>
    /// Clears all moves from the history.
    /// </summary>
    public void ClearHistory()
    {
        _history.Clear();
    }

    /// <summary>
    /// Returns whether there are any moves in the history.
    /// </summary>
    public bool HasMoves => _history.Count > 0;
}
