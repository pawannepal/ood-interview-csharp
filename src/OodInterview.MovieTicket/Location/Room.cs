namespace OodInterview.MovieTicket.Location;

/// <summary>
/// Represents a cinema room with a layout.
/// </summary>
public class Room
{
    /// <summary>
    /// Creates a new room with the specified room number and layout.
    /// </summary>
    /// <param name="roomNumber">The room number/identifier.</param>
    /// <param name="layout">The seating layout of the room.</param>
    public Room(string roomNumber, Layout layout)
    {
        RoomNumber = roomNumber;
        Layout = layout;
    }

    /// <summary>
    /// Gets the room number.
    /// </summary>
    public string RoomNumber { get; }

    /// <summary>
    /// Gets the seating layout.
    /// </summary>
    public Layout Layout { get; }
}
