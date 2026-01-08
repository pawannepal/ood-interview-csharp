using OodInterview.MovieTicket.Location;
using OodInterview.MovieTicket.Showing;

namespace OodInterview.MovieTicket.Ticket;

/// <summary>
/// Manages the relationships between movies, screenings, and tickets in the booking system.
/// </summary>
public class ScreeningManager
{
    private readonly Dictionary<Movie, List<Screening>> _screeningsByMovie = [];
    private readonly Dictionary<Screening, List<Ticket>> _ticketsByScreening = [];

    /// <summary>
    /// Adds a screening for a movie.
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <param name="screening">The screening to add.</param>
    public void AddScreening(Movie movie, Screening screening)
    {
        if (!_screeningsByMovie.TryGetValue(movie, out var screenings))
        {
            screenings = [];
            _screeningsByMovie[movie] = screenings;
        }
        screenings.Add(screening);
    }

    /// <summary>
    /// Returns all screenings for a specific movie.
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <returns>A list of screenings for the movie.</returns>
    public IReadOnlyList<Screening> GetScreeningsForMovie(Movie movie)
    {
        return _screeningsByMovie.GetValueOrDefault(movie, []).AsReadOnly();
    }

    /// <summary>
    /// Adds a ticket for a screening.
    /// </summary>
    /// <param name="screening">The screening.</param>
    /// <param name="ticket">The ticket to add.</param>
    public void AddTicket(Screening screening, Ticket ticket)
    {
        if (!_ticketsByScreening.TryGetValue(screening, out var tickets))
        {
            tickets = [];
            _ticketsByScreening[screening] = tickets;
        }
        tickets.Add(ticket);
    }

    /// <summary>
    /// Returns all tickets sold for a specific screening.
    /// </summary>
    /// <param name="screening">The screening.</param>
    /// <returns>A list of tickets for the screening.</returns>
    public IReadOnlyList<Ticket> GetTicketsForScreening(Screening screening)
    {
        return _ticketsByScreening.GetValueOrDefault(screening, []).AsReadOnly();
    }

    /// <summary>
    /// Calculates which seats are still available for a screening.
    /// </summary>
    /// <param name="screening">The screening.</param>
    /// <returns>A list of available seats.</returns>
    public List<Seat> GetAvailableSeats(Screening screening)
    {
        var allSeats = screening.Room.Layout.GetAllSeats();
        var bookedTickets = GetTicketsForScreening(screening);

        var availableSeats = new List<Seat>(allSeats);
        foreach (var ticket in bookedTickets)
        {
            availableSeats.Remove(ticket.Seat);
        }
        return availableSeats;
    }
}
