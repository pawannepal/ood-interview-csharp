using OodInterview.MovieTicket.Location;
using OodInterview.MovieTicket.Showing;
using OodInterview.MovieTicket.Ticket;

namespace OodInterview.MovieTicket;

/// <summary>
/// Manages the complete movie booking system operations.
/// </summary>
public class MovieBookingSystem
{
    private readonly List<Movie> _movies = [];
    private readonly List<Cinema> _cinemas = [];
    private readonly ScreeningManager _screeningManager = new();

    /// <summary>
    /// Adds a movie to the system.
    /// </summary>
    /// <param name="movie">The movie to add.</param>
    public void AddMovie(Movie movie)
    {
        _movies.Add(movie);
    }

    /// <summary>
    /// Adds a cinema to the system.
    /// </summary>
    /// <param name="cinema">The cinema to add.</param>
    public void AddCinema(Cinema cinema)
    {
        _cinemas.Add(cinema);
    }

    /// <summary>
    /// Gets all movies in the system.
    /// </summary>
    public IReadOnlyList<Movie> AllMovies => _movies.AsReadOnly();

    /// <summary>
    /// Gets all cinemas in the system.
    /// </summary>
    public IReadOnlyList<Cinema> AllCinemas => _cinemas.AsReadOnly();

    /// <summary>
    /// Returns all screenings for a specific movie.
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <returns>A list of screenings for the movie.</returns>
    public IReadOnlyList<Screening> GetScreeningsForMovie(Movie movie)
    {
        return _screeningManager.GetScreeningsForMovie(movie);
    }

    /// <summary>
    /// Returns all available seats for a screening.
    /// </summary>
    /// <param name="screening">The screening.</param>
    /// <returns>A list of available seats.</returns>
    public List<Seat> GetAvailableSeats(Screening screening)
    {
        return _screeningManager.GetAvailableSeats(screening);
    }

    /// <summary>
    /// Books a ticket for a specific seat at a screening.
    /// </summary>
    /// <param name="screening">The screening.</param>
    /// <param name="seat">The seat to book.</param>
    public void BookTicket(Screening screening, Seat seat)
    {
        var price = seat.PricingStrategy?.Price ?? 0;
        var ticket = new Ticket.Ticket(screening, seat, price);
        _screeningManager.AddTicket(screening, ticket);
    }

    /// <summary>
    /// Adds a screening for a movie.
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <param name="screening">The screening to add.</param>
    public void AddScreening(Movie movie, Screening screening)
    {
        _screeningManager.AddScreening(movie, screening);
    }

    /// <summary>
    /// Returns the number of tickets sold for a screening.
    /// </summary>
    /// <param name="screening">The screening.</param>
    /// <returns>The number of tickets sold.</returns>
    public int GetTicketCount(Screening screening)
    {
        return _screeningManager.GetTicketsForScreening(screening).Count;
    }

    /// <summary>
    /// Returns the list of tickets for a screening.
    /// </summary>
    /// <param name="screening">The screening.</param>
    /// <returns>A list of tickets for the screening.</returns>
    public IReadOnlyList<Ticket.Ticket> GetTicketsForScreening(Screening screening)
    {
        return _screeningManager.GetTicketsForScreening(screening);
    }
}
