namespace OodInterview.TicTacToe;

/// <summary>
/// Tracks player scores/ratings.
/// Winner gets +1 point, loser gets -1 point, no change for draw.
/// </summary>
public class ScoreTracker
{
    private readonly Dictionary<Player, int> _playerRatings = [];

    /// <summary>
    /// Updates player ratings based on game outcome.
    /// </summary>
    public void ReportGameResult(Player player1, Player player2, Player? winningPlayer)
    {
        if (winningPlayer != null)
        {
            var loser = player1 == winningPlayer ? player2 : player1;
            
            _playerRatings.TryAdd(winningPlayer, 0);
            _playerRatings[winningPlayer]++;
            
            _playerRatings.TryAdd(loser, 0);
            _playerRatings[loser]--;
        }
    }

    /// <summary>
    /// Returns a list of players sorted by their ratings in descending order.
    /// </summary>
    public List<Player> GetTopPlayers()
    {
        return _playerRatings
            .OrderByDescending(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .ToList();
    }

    /// <summary>
    /// Returns the rank of a player (1-based) based on their rating.
    /// </summary>
    public int GetRank(Player player)
    {
        var sortedPlayers = GetTopPlayers();
        return sortedPlayers.IndexOf(player) + 1;
    }

    /// <summary>
    /// Returns the complete map of player ratings.
    /// </summary>
    public IReadOnlyDictionary<Player, int> PlayerRatings => _playerRatings;
}
