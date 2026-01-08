namespace OodInterview.MovieTicket.Location;

/// <summary>
/// Represents a cinema with multiple rooms.
/// </summary>
public class Cinema
{
    private readonly List<Room> _rooms = [];

    /// <summary>
    /// Creates a new cinema with the specified name and location.
    /// </summary>
    /// <param name="name">The name of the cinema.</param>
    /// <param name="location">The location of the cinema.</param>
    public Cinema(string name, string location)
    {
        Name = name;
        Location = location;
    }

    /// <summary>
    /// Gets the cinema name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the cinema location.
    /// </summary>
    public string Location { get; }

    /// <summary>
    /// Gets all rooms in this cinema.
    /// </summary>
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    /// <summary>
    /// Adds a room to this cinema.
    /// </summary>
    /// <param name="room">The room to add.</param>
    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }
}
