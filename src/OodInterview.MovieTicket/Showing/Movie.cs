namespace OodInterview.MovieTicket.Showing;

/// <summary>
/// Represents a movie.
/// </summary>
public class Movie
{
    /// <summary>
    /// Creates a new movie with the specified details.
    /// </summary>
    /// <param name="title">The movie title.</param>
    /// <param name="genre">The movie genre.</param>
    /// <param name="durationInMinutes">The movie duration in minutes.</param>
    public Movie(string title, string genre, int durationInMinutes)
    {
        Title = title;
        Genre = genre;
        DurationInMinutes = durationInMinutes;
    }

    /// <summary>
    /// Gets the movie title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the movie genre.
    /// </summary>
    public string Genre { get; }

    /// <summary>
    /// Gets the movie duration in minutes.
    /// </summary>
    public int DurationInMinutes { get; }

    /// <summary>
    /// Gets the movie duration as a TimeSpan.
    /// </summary>
    public TimeSpan Duration => TimeSpan.FromMinutes(DurationInMinutes);
}
