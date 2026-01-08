# Blackjack - OOD Interview

A .NET Core 10 implementation of a Blackjack card game, ported from the ByteByteGoHq/ood-interview Java repository.

## Overview

This project demonstrates object-oriented design principles through a classic Blackjack card game implementation. The game supports multiple players competing against a dealer, with full betting and standard Blackjack rules.

## Project Structure

```
OodInterview.Blackjack/
├── Action.cs                  # Player action enum (Bet, Hit, Stand)
├── GamePhase.cs               # Game phase enum (Started, BetPlaced, etc.)
├── BlackJackGame.cs           # Main game controller
├── Deck/
│   ├── Card.cs                # Playing card representation
│   ├── Deck.cs                # 52-card deck with shuffle/draw
│   ├── Rank.cs                # Card rank enum with values
│   └── Suit.cs                # Card suit enum
└── Player/
    ├── Hand.cs                # Player's hand with value calculation
    ├── IPlayer.cs             # Player interface
    ├── RealPlayer.cs          # Human player implementation
    └── DealerPlayer.cs        # Dealer implementation
```

## Design Patterns

### 1. Interface Segregation

The `IPlayer` interface defines the contract for all players:

```csharp
public interface IPlayer
{
    void PlaceBet(int bet);
    void LoseBet();
    void ReturnBet();
    void Payout();
    bool IsBust { get; }
    Hand Hand { get; }
    int Balance { get; }
    string Name { get; }
    int CurrentBet { get; }
}
```

Both `RealPlayer` and `DealerPlayer` implement this interface, allowing the game to treat all players uniformly while each has different behavior (e.g., dealer doesn't bet).

### 2. Encapsulation

The `Hand` class encapsulates the complexity of Blackjack hand value calculation:

- **Ace Flexibility**: Aces can be worth 1 or 11
- **Multiple Possible Values**: The hand tracks all possible totals
- **Bust Detection**: Automatically determines if all possible values exceed 21

```csharp
public class Hand
{
    private readonly SortedSet<int> _possibleValues = [];
    
    public int BestValue
    {
        get
        {
            // Returns the highest value <= 21, or minimum if all bust
            var validValues = _possibleValues.Where(v => v <= 21).ToList();
            return validValues.Count > 0 ? validValues.Max() : _possibleValues.Min;
        }
    }
    
    public bool IsBust => _possibleValues.Count > 0 && _possibleValues.Min > 21;
}
```

### 3. State Management with Enums

The game uses enums to manage game phases and player actions:

```csharp
public enum GamePhase
{
    Started,        // Game initialized, waiting for bets
    BetPlaced,      // All bets placed, ready to deal
    InitialCardDrawn, // Initial cards dealt
    PlayerTurn,     // Players taking actions
    End             // Game complete, bets resolved
}

public enum Action
{
    Bet,
    Hit,
    Stand
}
```

### 4. Extension Methods for Enum Values

Card rank values are provided through extension methods, keeping the enum clean while providing behavior:

```csharp
public enum Rank { Ace, Two, Three, ... King }

public static class RankExtensions
{
    public static int[] GetRankValues(this Rank rank) => rank switch
    {
        Rank.Ace => [1, 11],
        Rank.Two => [2],
        // Face cards
        Rank.Jack => [10],
        Rank.Queen => [10],
        Rank.King => [10],
        _ => throw new ArgumentOutOfRangeException()
    };
}
```

## Game Rules

### Standard Blackjack Rules Implemented

1. **Objective**: Get as close to 21 as possible without going over
2. **Card Values**:
   - Number cards (2-10): Face value
   - Face cards (J, Q, K): 10
   - Aces: 1 or 11 (flexible)
3. **Betting**: Players must bet before cards are dealt
4. **Actions**:
   - **Hit**: Draw another card
   - **Stand**: Keep current hand
5. **Dealer Rules**: Dealer must hit on 16 or below, stand on 17 or above
6. **Outcomes**:
   - **Win**: Player has higher value than dealer (or dealer busts)
   - **Lose**: Dealer has higher value (or player busts)
   - **Push (Tie)**: Equal values - bet is returned

### Payout Rules

- **Win**: Player receives 2x their bet (original bet + equal winnings)
- **Lose**: Player loses their bet
- **Push**: Player's bet is returned

## Key Classes

### BlackJackGame

The main game controller that orchestrates:
- Player registration
- Deck management
- Turn order
- Bet resolution

```csharp
var player = new RealPlayer("Alice", 100);
var game = new BlackJackGame([player]);

game.Bet(player, 10);      // Place bet
game.DealInitialCards();   // Deal 2 cards each
game.Hit(player);          // Draw another card
game.Stand(player);        // End player's turn
game.DealerTurn();         // Dealer plays
// Bets are automatically resolved
```

### Deck

A standard 52-card deck with:
- Initialization with all card combinations
- Shuffling with optional seed for reproducibility
- Draw functionality with empty deck detection

### Hand

Manages a player's cards with:
- Automatic value calculation
- Ace flexibility handling
- Bust detection

## Test Cases

The test suite covers:

1. **TestDealerWins** - Dealer beats player with higher hand
2. **TestTie** - Equal hands result in bet return
3. **TestPlayerStandsAndWins** - Player wins by standing with higher hand
4. **TestPlayerHitsAndWins** - Player wins after hitting, dealer busts
5. **TestPlayerHitsAndBusts** - Player busts after hitting
6. **TestMultiplePlayersAndMultipleHits** - Multiple players with multiple actions
7. **TestDealerBustsPlayerWins** - Dealer busts, player wins

## Running the Project

```bash
# Build the project
dotnet build src/OodInterview.Blackjack

# Run tests
dotnet test tests/OodInterview.Blackjack.Tests

# Run with verbose output
dotnet test tests/OodInterview.Blackjack.Tests --verbosity normal
```

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `SortedSet<Integer>` | `SortedSet<int>` |
| `getPossibleValues().last()` | `BestValue` property |
| `Collections.unmodifiableList()` | `.AsReadOnly()` |
| Interface `Player` | Interface `IPlayer` |
| `player.bet(amount)` | `player.PlaceBet(amount)` |
| Package-private members | `internal` access |

## Future Enhancements

Potential extensions to the design:

1. **Split Hands**: Allow splitting pairs
2. **Double Down**: Double bet and receive one more card
3. **Insurance**: Side bet when dealer shows Ace
4. **Multiple Decks**: Shoe with multiple decks
5. **Card Counting Prevention**: Shuffle when deck is low
