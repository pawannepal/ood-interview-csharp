namespace OodInterview.MovieTicket.Location;

/// <summary>
/// Represents the seating layout of a cinema room.
/// </summary>
public class Layout
{
    private readonly Dictionary<string, Seat> _seatsByNumber = [];
    private readonly Dictionary<int, Dictionary<int, Seat>> _seatsByPosition = [];

    /// <summary>
    /// Creates a new layout with the specified number of rows and columns.
    /// </summary>
    /// <param name="rows">Number of rows.</param>
    /// <param name="columns">Number of columns.</param>
    public Layout(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        InitializeLayout();
    }

    /// <summary>
    /// Gets the number of rows.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the number of columns.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Creates seats for all positions with default null pricing.
    /// </summary>
    private void InitializeLayout()
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                var seatNumber = $"{i}-{j}";
                AddSeat(seatNumber, i, j, new Seat(seatNumber, null));
            }
        }
    }

    /// <summary>
    /// Adds a seat to the layout.
    /// </summary>
    /// <param name="seatNumber">The seat number.</param>
    /// <param name="row">The row position.</param>
    /// <param name="column">The column position.</param>
    /// <param name="seat">The seat to add.</param>
    public void AddSeat(string seatNumber, int row, int column, Seat seat)
    {
        _seatsByNumber[seatNumber] = seat;

        if (!_seatsByPosition.TryGetValue(row, out var rowSeats))
        {
            rowSeats = [];
            _seatsByPosition[row] = rowSeats;
        }
        rowSeats[column] = seat;
    }

    /// <summary>
    /// Gets a seat by its seat number.
    /// </summary>
    /// <param name="seatNumber">The seat number.</param>
    /// <returns>The seat, or null if not found.</returns>
    public Seat? GetSeatByNumber(string seatNumber)
    {
        return _seatsByNumber.GetValueOrDefault(seatNumber);
    }

    /// <summary>
    /// Gets a seat by its row and column position.
    /// </summary>
    /// <param name="row">The row position.</param>
    /// <param name="column">The column position.</param>
    /// <returns>The seat, or null if not found.</returns>
    public Seat? GetSeatByPosition(int row, int column)
    {
        if (_seatsByPosition.TryGetValue(row, out var rowSeats))
        {
            return rowSeats.GetValueOrDefault(column);
        }
        return null;
    }

    /// <summary>
    /// Gets all seats in the layout.
    /// </summary>
    /// <returns>A list of all seats.</returns>
    public IReadOnlyList<Seat> GetAllSeats()
    {
        return _seatsByNumber.Values.ToList();
    }
}
