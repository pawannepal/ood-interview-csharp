namespace OodInterview.Blackjack;

/// <summary>
/// Main game class that manages the Blackjack game flow.
/// </summary>
public class BlackJackGame
{
    private readonly Deck _deck = new();
    private readonly List<IPlayer> _players = [];
    private readonly IPlayer _dealer = new DealerPlayer();
    private IPlayer? _currentPlayer = null;

    /// <summary>
    /// Tracks the current status of each player's turn (e.g., HIT or STAND).
    /// </summary>
    internal Dictionary<IPlayer, Action?> PlayerTurnStatusMap { get; } = [];
    internal GamePhase CurrentPhase { get; set; } = GamePhase.Started;

    /// <summary>
    /// Constructor for BlackJackGame.
    /// </summary>
    public BlackJackGame(IEnumerable<IPlayer> players)
    {
        foreach (var player in players)
        {
            ArgumentNullException.ThrowIfNull(player);
            _players.Add(player);
            PlayerTurnStatusMap[player] = null;
        }
        PlayerTurnStatusMap[_dealer] = null;
        _deck.Shuffle(); // Shuffle the deck when game starts
    }

    /// <summary>
    /// Determines the next player who can take an action (i.e., has not stood or bust).
    /// If the current player is the dealer, it triggers the dealer's turn.
    /// </summary>
    public IPlayer? GetNextEligiblePlayer()
    {
        // If current player hasn't stood or bust, they can continue their turn
        if (_currentPlayer != null &&
            PlayerTurnStatusMap[_currentPlayer] != Action.Stand &&
            !_currentPlayer.IsBust)
        {
            return _currentPlayer;
        }

        // Find the first player who hasn't stood or bust
        if (_currentPlayer == null)
        {
            foreach (var player in _players)
            {
                if (PlayerTurnStatusMap[player] != Action.Stand && !player.IsBust)
                {
                    _currentPlayer = player;
                    return _currentPlayer;
                }
            }
        }

        // Else, find the next player after the current one who hasn't stood or bust
        int currentPlayerIndex = _players.IndexOf(_currentPlayer!);
        for (int i = currentPlayerIndex + 1; i < _players.Count; i++)
        {
            var player = _players[i];
            if (PlayerTurnStatusMap[player] != Action.Stand && !player.IsBust)
            {
                _currentPlayer = player;
                return _currentPlayer;
            }
        }

        // All players are done, dealer's turn
        return _dealer;
    }

    /// <summary>
    /// Executes the dealer's turn according to Blackjack rules.
    /// Dealer hits if below 17.
    /// </summary>
    public void DealerTurn()
    {
        // Dealer hits if below 17
        while (_dealer.Hand.BestValue < 17)
        {
            var newDraw = _deck.Draw();
            if (newDraw != null)
            {
                _dealer.Hand.AddCard(newDraw);
            }
        }
        PlayerTurnStatusMap[_dealer] = Action.Stand;
        CheckGameEndCondition();
    }

    /// <summary>
    /// Starts a new round by resetting the deck and all hands.
    /// </summary>
    public void StartNewRound()
    {
        _deck.Reset();
        foreach (var player in PlayerTurnStatusMap.Keys)
        {
            player.Hand.Clear();
        }
        _dealer.Hand.Clear();

        // Reset all turn statuses to null
        foreach (var player in PlayerTurnStatusMap.Keys.ToList())
        {
            PlayerTurnStatusMap[player] = null;
        }
        _currentPlayer = null;
        CurrentPhase = GamePhase.Started;
    }

    /// <summary>
    /// Deals initial cards to all players and the dealer.
    /// </summary>
    public void DealInitialCards()
    {
        if (CurrentPhase != GamePhase.BetPlaced)
        {
            throw new InvalidOperationException("All players must bet before dealing");
        }

        // Deal first card to each real player in order
        foreach (var player in _players)
        {
            var card = _deck.Draw();
            if (card != null) player.Hand.AddCard(card);
        }

        // Deal first card to dealer
        var dealerCard1 = _deck.Draw();
        if (dealerCard1 != null) _dealer.Hand.AddCard(dealerCard1);

        // Deal second card to each real player in order
        foreach (var player in _players)
        {
            var card = _deck.Draw();
            if (card != null) player.Hand.AddCard(card);
        }

        // Deal second card to dealer
        var dealerCard2 = _deck.Draw();
        if (dealerCard2 != null) _dealer.Hand.AddCard(dealerCard2);

        CurrentPhase = GamePhase.InitialCardDrawn;
    }

    /// <summary>
    /// Places a bet for a player.
    /// </summary>
    public void Bet(IPlayer player, int bet)
    {
        if (CurrentPhase != GamePhase.Started)
        {
            throw new InvalidOperationException("Bets must be placed at the start of the round");
        }

        player.PlaceBet(bet);

        // Transition to BetPlaced once all players have bet
        if (_players.All(p => p.CurrentBet > 0))
        {
            CurrentPhase = GamePhase.BetPlaced;
        }
    }

    /// <summary>
    /// Player requests to hit (draw another card).
    /// </summary>
    public void Hit(IPlayer player)
    {
        if (PlayerTurnStatusMap[player] == Action.Stand)
        {
            throw new InvalidOperationException("Player has already stood");
        }
        if (player.IsBust)
        {
            throw new InvalidOperationException("Player is already bust");
        }

        var drawnCard = _deck.Draw();
        if (drawnCard != null)
        {
            player.Hand.AddCard(drawnCard);
        }
        PlayerTurnStatusMap[player] = Action.Hit;
    }

    /// <summary>
    /// Player requests to stand (no more cards).
    /// </summary>
    public void Stand(IPlayer player)
    {
        PlayerTurnStatusMap[player] = Action.Stand;
        CheckGameEndCondition();
    }

    /// <summary>
    /// Checks if the game has ended (all players done), then resolves bets
    /// by comparing each player's hand to the dealer's.
    /// </summary>
    private void CheckGameEndCondition()
    {
        bool allPlayersDone = _players.All(p =>
            PlayerTurnStatusMap[p] == Action.Stand || p.IsBust);

        // Also check if dealer has completed their turn
        bool dealerDone = PlayerTurnStatusMap[_dealer] == Action.Stand;

        if (!allPlayersDone || !dealerDone)
        {
            return;
        }

        int dealerValue = _dealer.Hand.BestValue;
        bool dealerBusts = _dealer.IsBust;

        foreach (var player in _players)
        {
            if (player.IsBust)
            {
                player.LoseBet();
            }
            else
            {
                int playerValue = player.Hand.BestValue;
                if (dealerBusts || playerValue > dealerValue)
                {
                    player.Payout();
                }
                else if (playerValue == dealerValue)
                {
                    player.ReturnBet();
                }
                else
                {
                    player.LoseBet();
                }
            }
        }
        CurrentPhase = GamePhase.End;
    }

    /// <summary>
    /// Returns a string representation of the current game state.
    /// </summary>
    public string GetStateString()
    {
        var sb = new System.Text.StringBuilder();
        foreach (var player in PlayerTurnStatusMap.Keys)
        {
            sb.Append(player.Name)
              .Append(": ")
              .Append(player.Hand.ToString())
              .Append(", ")
              .Append(PlayerTurnStatusMap[player])
              .AppendLine();
        }
        return sb.ToString();
    }

    /// <summary>
    /// Gets the deck (internal for testing).
    /// </summary>
    internal Deck Deck => _deck;

    /// <summary>
    /// Gets the current turn player.
    /// </summary>
    public IPlayer? CurrentTurnPlayer => _currentPlayer;

    /// <summary>
    /// Gets the dealer.
    /// </summary>
    public IPlayer Dealer => _dealer;
}
