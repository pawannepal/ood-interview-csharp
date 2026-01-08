using OodInterview.MovieTicket.Location;

namespace OodInterview.MovieTicket.Showing;

/// <summary>
/// Represents a scheduled screening of a movie in a specific cinema room.
/// </summary>
public class Screening
{
    /// <summary>
    /// Creates a new screening with the specified details.
    /// </summary>
    /// <param name="movie">The movie being screened.</param>
    /// <param name="room">The room where the screening takes place.</param>
    /// <param name="startTime">The start time of the screening.</param>
    /// <param name="endTime">The end time of the screening.</param>
    public Screening(Movie movie, Room room, DateTime startTime, DateTime endTime)
    {
        Movie = movie;
        Room = room;
        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>
    /// Gets the movie being screened.
    /// </summary>
    public Movie Movie { get; }

    /// <summary>
    /// Gets the room where the screening takes place.
    /// </summary>
    public Room Room { get; }

    /// <summary>
    /// Gets the start time of the screening.
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    /// Gets the end time of the screening.
    /// </summary>
    public DateTime EndTime { get; }

    /// <summary>
    /// Gets the duration of the screening.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;
}
