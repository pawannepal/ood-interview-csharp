using OodInterview.TicTacToe;

namespace OodInterview.TicTacToe.Tests;

public class TicTacToeTests
{
    [Fact]
    public void TestPlay_PlayerXWins()
    {
        // Arrange
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');
        var game = new Game(playerX, playerY);

        // Assert initial state
        Assert.Equal(GameCondition.InProgress, game.GetGameStatus());
        Assert.Equal(playerX, game.GetCurrentPlayer());

        // Make moves - X wins with top row
        game.MakeMove(0, 0, game.GetCurrentPlayer()); // X at (0,0)
        Assert.Equal(playerY, game.GetCurrentPlayer());
        
        game.MakeMove(1, 0, game.GetCurrentPlayer()); // Y at (1,0)
        game.MakeMove(0, 1, game.GetCurrentPlayer()); // X at (0,1)
        game.MakeMove(1, 1, game.GetCurrentPlayer()); // Y at (1,1)
        game.MakeMove(0, 2, game.GetCurrentPlayer()); // X at (0,2) - X wins!

        // Assert game ended and X won
        Assert.Equal(GameCondition.Ended, game.GetGameStatus());
        
        var scoreTracker = game.GetScoreTracker();
        Assert.Equal(1, scoreTracker.PlayerRatings[playerX]);
        Assert.Equal(-1, scoreTracker.PlayerRatings[playerY]);
        Assert.Equal(1, scoreTracker.GetRank(playerX));
        Assert.Equal(2, scoreTracker.GetRank(playerY));
        Assert.Equal(2, scoreTracker.GetTopPlayers().Count);
    }

    [Fact]
    public void TestDuplicatePlay_ShouldThrow()
    {
        // Arrange
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');
        var game = new Game(playerX, playerY);

        // Act - make first move
        game.MakeMove(1, 1, game.GetCurrentPlayer()); // X at center

        // Assert - duplicate move throws
        Assert.Throws<ArgumentException>(() => game.MakeMove(1, 1, game.GetCurrentPlayer()));
    }

    [Fact]
    public void TestWrongPlayer_ShouldThrow()
    {
        // Arrange
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');
        var game = new Game(playerX, playerY);

        // Assert - wrong player throws
        Assert.Throws<ArgumentException>(() => game.MakeMove(0, 0, playerY));
    }

    [Fact]
    public void TestGameEndedCannotMove()
    {
        // Arrange
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');
        var game = new Game(playerX, playerY);

        // Play until X wins
        game.MakeMove(0, 0, game.GetCurrentPlayer()); // X
        game.MakeMove(1, 0, game.GetCurrentPlayer()); // Y
        game.MakeMove(0, 1, game.GetCurrentPlayer()); // X
        game.MakeMove(1, 1, game.GetCurrentPlayer()); // Y
        game.MakeMove(0, 2, game.GetCurrentPlayer()); // X wins

        // Assert - cannot move after game ended
        Assert.Throws<InvalidOperationException>(() => game.MakeMove(2, 2, playerY));
    }

    [Fact]
    public void TestDrawGame()
    {
        // Arrange
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');
        var game = new Game(playerX, playerY);

        // Play a draw game
        // X O X
        // X X O
        // O X O
        game.MakeMove(0, 0, game.GetCurrentPlayer()); // X
        game.MakeMove(0, 1, game.GetCurrentPlayer()); // O
        game.MakeMove(0, 2, game.GetCurrentPlayer()); // X
        game.MakeMove(1, 2, game.GetCurrentPlayer()); // O
        game.MakeMove(1, 0, game.GetCurrentPlayer()); // X
        game.MakeMove(2, 0, game.GetCurrentPlayer()); // O
        game.MakeMove(1, 1, game.GetCurrentPlayer()); // X
        game.MakeMove(2, 2, game.GetCurrentPlayer()); // O
        game.MakeMove(2, 1, game.GetCurrentPlayer()); // X

        // Assert - game ended in draw
        Assert.Equal(GameCondition.Ended, game.GetGameStatus());
        Assert.Null(game.Board.GetWinner());
    }

    [Fact]
    public void TestStartNewGame_ResetsBoard()
    {
        // Arrange
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');
        var game = new Game(playerX, playerY);

        // Make some moves
        game.MakeMove(0, 0, game.GetCurrentPlayer());
        game.MakeMove(1, 1, game.GetCurrentPlayer());

        // Act - start new game
        game.StartNewGame(playerX, playerY);

        // Assert - board is reset
        Assert.Equal(GameCondition.InProgress, game.GetGameStatus());
        Assert.Equal(playerX, game.GetCurrentPlayer());
        Assert.Null(game.Board.GetPlayerAt(0, 0));
        Assert.Null(game.Board.GetPlayerAt(1, 1));
    }
}

public class BoardTests
{
    [Fact]
    public void GetWinner_Row_ReturnsWinner()
    {
        // Arrange
        var board = new Board();
        var player = new Player("X", 'X');

        // Act - fill top row
        board.UpdateBoard(0, 0, player);
        board.UpdateBoard(0, 1, player);
        board.UpdateBoard(0, 2, player);

        // Assert
        Assert.Equal(player, board.GetWinner());
    }

    [Fact]
    public void GetWinner_Column_ReturnsWinner()
    {
        // Arrange
        var board = new Board();
        var player = new Player("X", 'X');

        // Act - fill left column
        board.UpdateBoard(0, 0, player);
        board.UpdateBoard(1, 0, player);
        board.UpdateBoard(2, 0, player);

        // Assert
        Assert.Equal(player, board.GetWinner());
    }

    [Fact]
    public void GetWinner_MainDiagonal_ReturnsWinner()
    {
        // Arrange
        var board = new Board();
        var player = new Player("X", 'X');

        // Act - fill main diagonal
        board.UpdateBoard(0, 0, player);
        board.UpdateBoard(1, 1, player);
        board.UpdateBoard(2, 2, player);

        // Assert
        Assert.Equal(player, board.GetWinner());
    }

    [Fact]
    public void GetWinner_AntiDiagonal_ReturnsWinner()
    {
        // Arrange
        var board = new Board();
        var player = new Player("X", 'X');

        // Act - fill anti-diagonal
        board.UpdateBoard(0, 2, player);
        board.UpdateBoard(1, 1, player);
        board.UpdateBoard(2, 0, player);

        // Assert
        Assert.Equal(player, board.GetWinner());
    }

    [Fact]
    public void GetWinner_NoWinner_ReturnsNull()
    {
        // Arrange
        var board = new Board();

        // Assert - empty board has no winner
        Assert.Null(board.GetWinner());
    }

    [Fact]
    public void IsFull_EmptyBoard_ReturnsFalse()
    {
        // Arrange
        var board = new Board();

        // Assert
        Assert.False(board.IsFull());
    }

    [Fact]
    public void IsFull_FullBoard_ReturnsTrue()
    {
        // Arrange
        var board = new Board();
        var playerX = new Player("X", 'X');
        var playerO = new Player("O", 'O');

        // Fill the board
        board.UpdateBoard(0, 0, playerX);
        board.UpdateBoard(0, 1, playerO);
        board.UpdateBoard(0, 2, playerX);
        board.UpdateBoard(1, 0, playerX);
        board.UpdateBoard(1, 1, playerO);
        board.UpdateBoard(1, 2, playerX);
        board.UpdateBoard(2, 0, playerO);
        board.UpdateBoard(2, 1, playerX);
        board.UpdateBoard(2, 2, playerO);

        // Assert
        Assert.True(board.IsFull());
    }

    [Fact]
    public void Reset_ClearsBoard()
    {
        // Arrange
        var board = new Board();
        var player = new Player("X", 'X');
        board.UpdateBoard(0, 0, player);
        board.UpdateBoard(1, 1, player);

        // Act
        board.Reset();

        // Assert
        Assert.Null(board.GetPlayerAt(0, 0));
        Assert.Null(board.GetPlayerAt(1, 1));
    }
}

public class ScoreTrackerTests
{
    [Fact]
    public void ReportGameResult_WinnerGetsPoint()
    {
        // Arrange
        var scoreTracker = new ScoreTracker();
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');

        // Act
        scoreTracker.ReportGameResult(playerX, playerY, playerX);

        // Assert
        Assert.Equal(1, scoreTracker.PlayerRatings[playerX]);
        Assert.Equal(-1, scoreTracker.PlayerRatings[playerY]);
    }

    [Fact]
    public void ReportGameResult_Draw_NoChange()
    {
        // Arrange
        var scoreTracker = new ScoreTracker();
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');

        // Act - draw (no winner)
        scoreTracker.ReportGameResult(playerX, playerY, null);

        // Assert - no ratings recorded
        Assert.Empty(scoreTracker.PlayerRatings);
    }

    [Fact]
    public void GetTopPlayers_SortedByRating()
    {
        // Arrange
        var scoreTracker = new ScoreTracker();
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');
        var playerZ = new Player("Z", 'Z');

        // X wins twice, Y wins once against Z
        scoreTracker.ReportGameResult(playerX, playerY, playerX);
        scoreTracker.ReportGameResult(playerX, playerZ, playerX);
        scoreTracker.ReportGameResult(playerY, playerZ, playerY);

        // Act
        var topPlayers = scoreTracker.GetTopPlayers();

        // Assert - X is first with +2, Y is second with 0 (1-1), Z is last with -2
        Assert.Equal(playerX, topPlayers[0]);
    }

    [Fact]
    public void GetRank_ReturnsCorrectRank()
    {
        // Arrange
        var scoreTracker = new ScoreTracker();
        var playerX = new Player("X", 'X');
        var playerY = new Player("Y", 'O');

        scoreTracker.ReportGameResult(playerX, playerY, playerX);

        // Act & Assert
        Assert.Equal(1, scoreTracker.GetRank(playerX));
        Assert.Equal(2, scoreTracker.GetRank(playerY));
    }
}

public class MoveHistoryTests
{
    [Fact]
    public void RecordMove_AddsToHistory()
    {
        // Arrange
        var history = new MoveHistory();
        var player = new Player("X", 'X');
        var move = new Move(0, 0, player);

        // Act
        history.RecordMove(move);

        // Assert
        Assert.True(history.HasMoves);
    }

    [Fact]
    public void UndoMove_ReturnsLastMove()
    {
        // Arrange
        var history = new MoveHistory();
        var player = new Player("X", 'X');
        var move1 = new Move(0, 0, player);
        var move2 = new Move(1, 1, player);
        history.RecordMove(move1);
        history.RecordMove(move2);

        // Act
        var undone = history.UndoMove();

        // Assert
        Assert.Equal(move2, undone);
    }

    [Fact]
    public void ClearHistory_RemovesAllMoves()
    {
        // Arrange
        var history = new MoveHistory();
        var player = new Player("X", 'X');
        history.RecordMove(new Move(0, 0, player));
        history.RecordMove(new Move(1, 1, player));

        // Act
        history.ClearHistory();

        // Assert
        Assert.False(history.HasMoves);
    }
}

public class PlayerTests
{
    [Fact]
    public void Player_HasCorrectProperties()
    {
        // Arrange & Act
        var player = new Player("Alice", 'X');

        // Assert
        Assert.Equal("Alice", player.Name);
        Assert.Equal('X', player.Symbol);
        Assert.Equal("Alice", player.ToString());
    }
}

public class MoveTests
{
    [Fact]
    public void Move_HasCorrectProperties()
    {
        // Arrange
        var player = new Player("X", 'X');

        // Act
        var move = new Move(1, 2, player);

        // Assert
        Assert.Equal(1, move.ColIndex);
        Assert.Equal(2, move.RowIndex);
        Assert.Equal(player, move.Player);
    }
}
