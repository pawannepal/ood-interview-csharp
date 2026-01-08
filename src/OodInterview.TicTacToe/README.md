# TicTacToe System

A classic Tic-Tac-Toe game implementation demonstrating object-oriented design principles with support for score tracking, move history, and undo functionality.

## Overview

This project implements a two-player Tic-Tac-Toe game with the following features:
- Turn-based gameplay on a 3x3 grid
- Winner detection (rows, columns, diagonals)
- Draw detection when board is full
- Move history with undo capability
- Score tracking across multiple games
- Player rankings

## Class Diagram

```
┌─────────────────────────────────────────────────────────────────────────┐
│                              Game                                        │
│─────────────────────────────────────────────────────────────────────────│
│ - _board: Board                                                          │
│ - _moveHistory: MoveHistory                                              │
│ - _scoreTracker: ScoreTracker                                            │
│ - _playerX: Player                                                       │
│ - _playerO: Player                                                       │
│ - _currentPlayer: Player                                                 │
│ - _gameCondition: GameCondition                                          │
│─────────────────────────────────────────────────────────────────────────│
│ + MakeMove(row, col, player): void                                       │
│ + UndoMove(): void                                                       │
│ + GetGameStatus(): GameCondition                                         │
│ + GetCurrentPlayer(): Player                                             │
│ + StartNewGame(playerX, playerO): void                                   │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ uses
            ┌───────────────────────┼───────────────────────┐
            ▼                       ▼                       ▼
┌───────────────────┐   ┌───────────────────┐   ┌───────────────────┐
│      Board        │   │   MoveHistory     │   │   ScoreTracker    │
│───────────────────│   │───────────────────│   │───────────────────│
│ - _grid: Player?[]│   │ - _moves: Stack   │   │ - PlayerRatings   │
│───────────────────│   │───────────────────│   │───────────────────│
│ + UpdateBoard()   │   │ + RecordMove()    │   │ + ReportResult()  │
│ + GetWinner()     │   │ + UndoMove()      │   │ + GetTopPlayers() │
│ + IsFull()        │   │ + ClearHistory()  │   │ + GetRank()       │
│ + Reset()         │   │ + HasMoves        │   │                   │
└───────────────────┘   └───────────────────┘   └───────────────────┘
            │                       │
            ▼                       ▼
    ┌───────────────┐       ┌───────────────┐
    │    Player     │       │     Move      │
    │───────────────│       │───────────────│
    │ + Name        │◄──────│ + ColIndex    │
    │ + Symbol      │       │ + RowIndex    │
    └───────────────┘       │ + Player      │
                            └───────────────┘

┌───────────────────────┐
│    GameCondition      │
│───────────────────────│
│ InProgress            │
│ Ended                 │
└───────────────────────┘
```

## Design Patterns Used

### 1. Encapsulation

The game logic is encapsulated within dedicated classes, each with a single responsibility:
- **Board**: Manages the 3x3 grid and winner detection
- **MoveHistory**: Tracks moves for undo functionality
- **ScoreTracker**: Maintains player rankings and scores
- **Game**: Orchestrates gameplay and enforces rules

### 2. Single Responsibility Principle (SRP)

Each class has one clear responsibility:
- `Board` - Grid state management and winner detection
- `MoveHistory` - Move tracking and undo support
- `ScoreTracker` - Player scoring and rankings
- `Player` - Player identity (name and symbol)
- `Move` - Represents a single move with position and player

### 3. Memento Pattern (Simplified)

The `MoveHistory` class implements a simplified version of the Memento pattern:
- Stores moves in a stack for chronological tracking
- Enables undo functionality by popping the last move
- Preserves game state history

```csharp
public class MoveHistory
{
    private readonly Stack<Move> _moves = new();
    
    public void RecordMove(Move move) => _moves.Push(move);
    public Move? UndoMove() => _moves.TryPop(out var move) ? move : null;
    public void ClearHistory() => _moves.Clear();
}
```

### 4. State Pattern (Implicit)

The `GameCondition` enum represents the game state:
- `InProgress` - Game is active, moves can be made
- `Ended` - Game has concluded (winner or draw)

The `Game` class transitions between states based on board conditions.

## Key Classes

### Game

The main controller that orchestrates gameplay:

```csharp
public class Game
{
    public void MakeMove(int rowIndex, int colIndex, Player player)
    {
        // 1. Validate game is in progress
        // 2. Validate correct player's turn
        // 3. Validate position is available
        // 4. Update board and record move
        // 5. Check for winner or draw
        // 6. Switch to next player
    }
    
    public void UndoMove()
    {
        // Revert last move from history
    }
}
```

### Board

Manages the 3x3 grid with winner detection:

```csharp
public class Board
{
    public Player? GetWinner()
    {
        // Check rows
        for (int row = 0; row < Size; row++)
            if (CheckLine(row, 0, 0, 1)) return GetPlayerAt(row, 0);
        
        // Check columns
        for (int col = 0; col < Size; col++)
            if (CheckLine(0, col, 1, 0)) return GetPlayerAt(0, col);
        
        // Check diagonals
        if (CheckLine(0, 0, 1, 1)) return GetPlayerAt(0, 0);
        if (CheckLine(0, 2, 1, -1)) return GetPlayerAt(0, 2);
        
        return null;
    }
}
```

### ScoreTracker

Tracks player performance across games:

```csharp
public class ScoreTracker
{
    public Dictionary<Player, int> PlayerRatings { get; } = new();
    
    public void ReportGameResult(Player playerX, Player playerO, Player? winner)
    {
        if (winner == null) return; // Draw
        
        var loser = winner == playerX ? playerO : playerX;
        AdjustRating(winner, 1);
        AdjustRating(loser, -1);
    }
}
```

## Gameplay Flow

```
┌──────────────────┐
│  Create Game     │
│  (Player X, O)   │
└────────┬─────────┘
         ▼
┌──────────────────┐
│  MakeMove()      │◄─────────────────┐
│  Player X/O      │                  │
└────────┬─────────┘                  │
         ▼                            │
    ┌────────────┐                    │
    │  Valid?    │──No──►Exception    │
    └─────┬──────┘                    │
          │Yes                        │
          ▼                           │
┌──────────────────┐                  │
│  Update Board    │                  │
│  Record Move     │                  │
└────────┬─────────┘                  │
         ▼                            │
    ┌────────────┐                    │
    │  Winner?   │──No──►Switch Player│
    └─────┬──────┘                    │
          │Yes                        │
          ▼                           │
┌──────────────────┐                  │
│  Report Result   │                  │
│  End Game        │                  │
└──────────────────┘
```

## Usage Example

```csharp
// Create players
var playerX = new Player("Alice", 'X');
var playerO = new Player("Bob", 'O');

// Start a new game
var game = new Game(playerX, playerO);

// Make moves (X always starts)
game.MakeMove(0, 0, playerX); // Top-left
game.MakeMove(1, 1, playerO); // Center
game.MakeMove(0, 1, playerX); // Top-center
game.MakeMove(2, 0, playerO); // Bottom-left
game.MakeMove(0, 2, playerX); // Top-right - X wins!

// Check game status
if (game.GetGameStatus() == GameCondition.Ended)
{
    var winner = game.Board.GetWinner();
    Console.WriteLine($"{winner?.Name} wins!");
}

// Check rankings
var topPlayers = game.GetScoreTracker().GetTopPlayers();
foreach (var player in topPlayers)
{
    int rank = game.GetScoreTracker().GetRank(player);
    int rating = game.GetScoreTracker().PlayerRatings[player];
    Console.WriteLine($"#{rank}: {player.Name} (Rating: {rating})");
}

// Start another game
game.StartNewGame(playerO, playerX); // O starts this time
```

## Winner Detection Logic

The board checks for winners in three ways:

1. **Rows**: All three cells in any row match
2. **Columns**: All three cells in any column match
3. **Diagonals**: All three cells in either diagonal match

```
Win Conditions:
Row 0:     Row 1:     Row 2:     Col 0:     Col 1:     Col 2:
X X X      . . .      . . .      X . .      . X .      . . X
. . .      X X X      . . .      X . .      . X .      . . X
. . .      . . .      X X X      X . .      . X .      . . X

Main Diag:              Anti-Diag:
X . .                   . . X
. X .                   . X .
. . X                   X . .
```

## Move Undo Feature

The game supports undoing moves through the `MoveHistory` class:

```csharp
// Make some moves
game.MakeMove(0, 0, playerX);
game.MakeMove(1, 1, playerO);

// Undo the last move
game.UndoMove(); // Removes O's move at (1,1)
game.UndoMove(); // Removes X's move at (0,0)
```

## Project Structure

```
OodInterview.TicTacToe/
├── Game.cs              # Main game controller
├── Board.cs             # 3x3 grid management
├── Player.cs            # Player identity
├── Move.cs              # Move representation
├── MoveHistory.cs       # Move tracking for undo
├── ScoreTracker.cs      # Player rankings
└── GameCondition.cs     # Game state enum
```

## Testing

Run the tests with:

```bash
dotnet test tests/OodInterview.TicTacToe.Tests
```

The test suite covers:
- Complete game flow with winner detection
- Duplicate move validation
- Wrong player validation
- Game ended state validation
- Draw game detection
- New game reset
- Board winner detection (rows, columns, diagonals)
- Score tracking
- Move history and undo
