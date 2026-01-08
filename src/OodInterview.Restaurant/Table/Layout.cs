namespace OodInterview.Restaurant.Table;

/// <summary>
/// Manages the collection of tables in the restaurant.
/// </summary>
public class Layout
{
    private readonly Dictionary<int, Table> _tablesById = [];
    private readonly SortedDictionary<int, HashSet<Table>> _tablesByCapacity = [];

    public Layout(IList<int> tableCapacities)
    {
        for (var i = 0; i < tableCapacities.Count; i++)
        {
            var capacity = tableCapacities[i];
            var table = new Table(i, capacity);
            _tablesById[i] = table;

            if (!_tablesByCapacity.TryGetValue(capacity, out var tables))
            {
                tables = [];
                _tablesByCapacity[capacity] = tables;
            }
            tables.Add(table);
        }
    }

    /// <summary>
    /// Finds the smallest available table that can accommodate a party.
    /// </summary>
    public Table? FindAvailableTable(int partySize, DateTime reservationTime)
    {
        foreach (var (capacity, tables) in _tablesByCapacity)
        {
            if (capacity >= partySize)
            {
                foreach (var table in tables)
                {
                    if (table.IsAvailableAt(reservationTime))
                    {
                        return table;
                    }
                }
            }
        }
        return null;
    }
}
