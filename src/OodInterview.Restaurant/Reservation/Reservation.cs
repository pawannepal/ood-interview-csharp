using OodInterview.Restaurant.Table;

namespace OodInterview.Restaurant.Reservation;

/// <summary>
/// Represents a reservation made at the restaurant.
/// </summary>
public class Reservation
{
    public Reservation(string partyName, int partySize, DateTime time, Table.Table assignedTable)
    {
        PartyName = partyName;
        PartySize = partySize;
        Time = time;
        AssignedTable = assignedTable;
    }

    public string PartyName { get; }
    public int PartySize { get; }
    public DateTime Time { get; }
    public Table.Table AssignedTable { get; }
}
