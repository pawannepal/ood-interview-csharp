using OodInterview.Blackjack;

namespace OodInterview.Blackjack.Tests;

public class BlackJackGameTests
{
    /// <summary>
    /// Helper to set up a deck with a known order for deterministic testing.
    /// </summary>
    private static void SetDeckOrder(BlackJackGame game, List<Card> cards)
    {
        game.Deck.Cards = new List<Card>(cards);
        game.Deck.NextCardIndex = 0;
    }

    /// <summary>
    /// Helper to advance the game until it is over.
    /// </summary>
    private static void PlayUntilGameEnd(BlackJackGame game)
    {
        while (game.CurrentPhase != GamePhase.End)
        {
            var nextPlayer = game.GetNextEligiblePlayer();
            if (nextPlayer == game.Dealer)
            {
                game.DealerTurn();
                break;
            }
            else if (nextPlayer != null)
            {
                game.Stand(nextPlayer);
            }
            else
            {
                break;
            }
        }
    }

    [Fact]
    public void TestDealerWins()
    {
        // Setup: Player has 18, Dealer has 20
        // Dealing order: P1, D1, P2, D2
        var a = new RealPlayer("A", 100);
        var game = new BlackJackGame([a]);

        SetDeckOrder(game, [
            new Card(Rank.Ten, Suit.Hearts),     // Player's first card (10)
            new Card(Rank.Ten, Suit.Spades),     // Dealer's first card (10)
            new Card(Rank.Eight, Suit.Clubs),    // Player's second card (8) -> Player = 18
            new Card(Rank.Ten, Suit.Diamonds)    // Dealer's second card (10) -> Dealer = 20
        ]);

        game.Bet(a, 10);
        game.DealInitialCards();
        game.Stand(a);
        game.DealerTurn();

        // Player loses: 100 - 10 = 90
        Assert.Equal(90, a.Balance);
    }

    [Fact]
    public void TestTie()
    {
        var a = new RealPlayer("A", 100);
        var game = new BlackJackGame([a]);

        // Dealing order: P1, D1, P2, D2
        // Both get 20
        SetDeckOrder(game, [
            new Card(Rank.Ten, Suit.Hearts),     // Player's first card (10)
            new Card(Rank.Ten, Suit.Spades),     // Dealer's first card (10)
            new Card(Rank.Ten, Suit.Clubs),      // Player's second card (10) -> Player = 20
            new Card(Rank.Ten, Suit.Diamonds)    // Dealer's second card (10) -> Dealer = 20
        ]);

        game.Bet(a, 10);
        game.DealInitialCards();
        game.Stand(a);
        game.DealerTurn();
        PlayUntilGameEnd(game);

        // Tie: bet returned, balance stays 100
        Assert.Equal(100, a.Balance);
    }

    [Fact]
    public void TestPlayerStandsAndWins()
    {
        var a = new RealPlayer("A", 100);
        var game = new BlackJackGame([a]);

        // Dealing order: P1, D1, P2, D2
        // Player gets 20, Dealer gets 18
        SetDeckOrder(game, [
            new Card(Rank.Ten, Suit.Hearts),     // Player's first card (10)
            new Card(Rank.Ten, Suit.Spades),     // Dealer's first card (10)
            new Card(Rank.Ten, Suit.Clubs),      // Player's second card (10) -> Player = 20
            new Card(Rank.Eight, Suit.Diamonds)  // Dealer's second card (8) -> Dealer = 18
        ]);

        game.Bet(a, 10);
        game.DealInitialCards();
        game.Stand(a);
        game.DealerTurn();
        PlayUntilGameEnd(game);

        // Player wins: 100 - 10 + 20 = 110
        Assert.Equal(110, a.Balance);
    }

    [Fact]
    public void TestPlayerHitsAndWins()
    {
        var a = new RealPlayer("A", 100);
        var game = new BlackJackGame([a]);

        // Dealing order: P1, D1, P2, D2, then hits
        // Player gets 5+5=10, hits 6=16
        // Dealer gets 10+6=16, hits and busts with 10 (26)
        SetDeckOrder(game, [
            new Card(Rank.Five, Suit.Hearts),    // Player's first card (5)
            new Card(Rank.Ten, Suit.Spades),     // Dealer's first card (10)
            new Card(Rank.Five, Suit.Clubs),     // Player's second card (5) -> Player = 10
            new Card(Rank.Six, Suit.Diamonds),   // Dealer's second card (6) -> Dealer = 16
            new Card(Rank.Six, Suit.Hearts),     // Player's hit card (6) -> Player = 16
            new Card(Rank.Ten, Suit.Diamonds)    // Dealer's hit card (10) -> Dealer = 26 (bust!)
        ]);

        game.Bet(a, 10);
        game.DealInitialCards();
        
        // Player hits: 5 + 5 + 6 = 16
        game.Hit(a);
        game.Stand(a);
        game.DealerTurn();

        // Dealer busts, player wins: 100 - 10 + 20 = 110
        Assert.Equal(110, a.Balance);
    }

    [Fact]
    public void TestPlayerHitsAndBusts()
    {
        var a = new RealPlayer("A", 100);
        var game = new BlackJackGame([a]);

        // Dealing order: P1, D1, P2, D2, P_hit
        // Player gets 10+10=20, hits 2=22 (bust)
        SetDeckOrder(game, [
            new Card(Rank.Ten, Suit.Hearts),     // Player's first card (10)
            new Card(Rank.Ten, Suit.Spades),     // Dealer's first card (10)
            new Card(Rank.Ten, Suit.Clubs),      // Player's second card (10) -> Player = 20
            new Card(Rank.Eight, Suit.Diamonds), // Dealer's second card (8) -> Dealer = 18
            new Card(Rank.Two, Suit.Hearts)      // Player's hit card (2) -> Player = 22 (bust!)
        ]);

        game.Bet(a, 10);
        game.DealInitialCards();
        
        // Player hits: 10 + 10 + 2 = 22 (bust!)
        game.Hit(a);
        game.DealerTurn();

        // Player busts: 100 - 10 = 90
        Assert.Equal(90, a.Balance);
    }

    [Fact]
    public void TestMultiplePlayersAndMultipleHits()
    {
        var a = new RealPlayer("A", 100);
        var b = new RealPlayer("B", 100);
        var game = new BlackJackGame([a, b]);

        // Dealing order for 2 players: PA1, PB1, D1, PA2, PB2, D2, then hits
        // Player A: 5+5=10, hits 3=13, hits 3=16
        // Player B: 5+10=15, hits 2=17
        // Dealer: 10+8=18
        SetDeckOrder(game, [
            new Card(Rank.Five, Suit.Hearts),    // Player A's first card (5)
            new Card(Rank.Five, Suit.Clubs),     // Player B's first card (5)
            new Card(Rank.Ten, Suit.Spades),     // Dealer's first card (10)
            new Card(Rank.Five, Suit.Diamonds),  // Player A's second card (5) -> A = 10
            new Card(Rank.Ten, Suit.Hearts),     // Player B's second card (10) -> B = 15
            new Card(Rank.Eight, Suit.Diamonds), // Dealer's second card (8) -> Dealer = 18
            new Card(Rank.Three, Suit.Hearts),   // Player A's first hit (3) -> A = 13
            new Card(Rank.Three, Suit.Clubs),    // Player A's second hit (3) -> A = 16
            new Card(Rank.Two, Suit.Spades),     // Player B's hit (2) -> B = 17
            new Card(Rank.Seven, Suit.Hearts)    // Extra card for dealer if needed
        ]);

        game.Bet(a, 10);
        game.Bet(b, 10);
        game.DealInitialCards();

        // Player A: 5 + 5 = 10
        game.Hit(a); // 10 + 3 = 13
        game.Hit(a); // 13 + 3 = 16
        game.Stand(a);

        // Player B: 5 + 10 = 15
        game.Hit(b); // 15 + 2 = 17
        game.Stand(b);

        game.DealerTurn();

        // Dealer has 10 + 8 = 18
        // Player A has 16 < 18 -> loses
        // Player B has 17 < 18 -> loses
        Assert.Equal(90, a.Balance);
        Assert.Equal(90, b.Balance);
    }

    [Fact]
    public void TestDealerBustsPlayerWins()
    {
        var a = new RealPlayer("A", 100);
        var game = new BlackJackGame([a]);

        // Dealing order: P1, D1, P2, D2, D_hit
        // Player: 10+8=18, stands
        // Dealer: 10+6=16, hits 10=26 (bust)
        SetDeckOrder(game, [
            new Card(Rank.Ten, Suit.Hearts),     // Player's first card (10)
            new Card(Rank.Ten, Suit.Spades),     // Dealer's first card (10)
            new Card(Rank.Eight, Suit.Clubs),    // Player's second card (8) -> Player = 18
            new Card(Rank.Six, Suit.Diamonds),   // Dealer's second card (6) -> Dealer = 16
            new Card(Rank.Ten, Suit.Diamonds)    // Dealer hits and busts (26)
        ]);

        game.Bet(a, 10);
        game.DealInitialCards();
        game.Stand(a);
        game.DealerTurn();

        // Dealer busts, player wins: 100 - 10 + 20 = 110
        Assert.Equal(110, a.Balance);
    }
}
