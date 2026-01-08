namespace OodInterview.TicTacToe;

/// <summary>
/// Main game controller for Tic-Tac-Toe.
/// </summary>
public class Game
{
    private readonly Board _board;
    private readonly ScoreTracker _scoreTracker;
    private readonly MoveHistory _moveHistory;
    private Player[] _players = [];
    private int _currentPlayerIndex;

    /// <summary>
    /// Creates a new game with two players.
    /// </summary>
    public Game(Player playerX, Player playerY)
    {
        _board = new Board();
        _scoreTracker = new ScoreTracker();
        _moveHistory = new MoveHistory();
        StartNewGame(playerX, playerY);
    }

    /// <summary>
    /// Resets the game state and initializes players for a new game.
    /// </summary>
    public void StartNewGame(Player playerX, Player playerY)
    {
        _board.Reset();
        _moveHistory.ClearHistory();
        _players = [playerX, playerY];
        _currentPlayerIndex = 0;
    }

    /// <summary>
    /// Processes a player's move, validates it, and updates game state.
    /// </summary>
    public void MakeMove(int colIndex, int rowIndex, Player player)
    {
        // Validate that game hasn't ended
        if (GetGameStatus() == GameCondition.Ended)
        {
            throw new InvalidOperationException("Game ended");
        }

        // Validate that it's the correct player's turn
        if (_players[_currentPlayerIndex] != player)
        {
            throw new ArgumentException("Not the current player");
        }

        // Validate that the position is not already taken
        if (_board.GetPlayerAt(colIndex, rowIndex) != null)
        {
            throw new ArgumentException("Board position is taken");
        }

        // Update the board with the player's move
        _board.UpdateBoard(colIndex, rowIndex, player);

        // Record the move in history
        var newMove = new Move(colIndex, rowIndex, player);
        _moveHistory.RecordMove(newMove);

        // Switch to the next player
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;

        // If game has ended, update the score
        if (GetGameStatus() == GameCondition.Ended)
        {
            _scoreTracker.ReportGameResult(_players[0], _players[1], _board.GetWinner());
        }
    }

    /// <summary>
    /// Reverts the last move made in the game.
    /// </summary>
    public void UndoMove()
    {
        // Check if game has ended to prevent undoing after winner is reported
        if (GetGameStatus() == GameCondition.Ended)
        {
            throw new InvalidOperationException("Game ended and winner already reported");
        }

        // Get the last move from history
        var lastMove = _moveHistory.UndoMove();

        // Update current player index to previous player
        if (_currentPlayerIndex == 0)
        {
            _currentPlayerIndex = _players.Length - 1;
        }
        else
        {
            _currentPlayerIndex--;
        }

        // Clear the board position of the undone move
        _board.UpdateBoard(lastMove.ColIndex, lastMove.RowIndex, null);
    }

    /// <summary>
    /// Determines if the game is in progress or has ended.
    /// </summary>
    public GameCondition GetGameStatus()
    {
        var winner = _board.GetWinner();
        if (winner != null)
        {
            return GameCondition.Ended;
        }
        return _board.IsFull() ? GameCondition.Ended : GameCondition.InProgress;
    }

    /// <summary>
    /// Returns a string representation of the current game state.
    /// </summary>
    public string GetStateString()
    {
        return $"Between players [{string.Join(", ", _players.Select(p => p.ToString()))}], state: {GetGameStatus()}";
    }

    /// <summary>
    /// Returns the player whose turn it is.
    /// </summary>
    public Player GetCurrentPlayer() => _players[_currentPlayerIndex];

    /// <summary>
    /// Prints the current state of the game for visualization.
    /// </summary>
    public void PrintGameState()
    {
        Console.WriteLine($"Game State: {GetStateString()}");
        Console.WriteLine($"Current Player: {GetCurrentPlayer()}");
        Console.WriteLine(_board.PrintBoard());
    }

    /// <summary>
    /// Returns the score tracker for accessing game statistics.
    /// </summary>
    public ScoreTracker GetScoreTracker() => _scoreTracker;

    /// <summary>
    /// Returns the game board.
    /// </summary>
    public Board Board => _board;
}
