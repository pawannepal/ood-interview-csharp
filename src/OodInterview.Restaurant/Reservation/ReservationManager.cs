using OodInterview.Restaurant.Table;

namespace OodInterview.Restaurant.Reservation;

/// <summary>
/// Manages all reservations for the restaurant and handles table assignment.
/// </summary>
public class ReservationManager
{
    private readonly Layout _layout;
    private readonly HashSet<Reservation> _reservations = [];

    public ReservationManager(Layout layout)
    {
        _layout = layout;
    }

    /// <summary>
    /// Finds potential time slots for a reservation within the given time range.
    /// </summary>
    public DateTime[] FindAvailableTimeSlots(DateTime rangeStart, DateTime rangeEnd, int partySize)
    {
        var possibleReservations = new List<DateTime>();
        var current = rangeStart;

        while (current <= rangeEnd)
        {
            var availableTable = _layout.FindAvailableTable(partySize, current);
            if (availableTable != null)
            {
                possibleReservations.Add(current);
            }
            current = current.AddHours(1);
        }

        return [.. possibleReservations];
    }

    /// <summary>
    /// Creates a reservation for a specific time, party size and name.
    /// </summary>
    public Reservation CreateReservation(string partyName, int partySize, DateTime desiredTime)
    {
        // Truncate to hours
        desiredTime = new DateTime(desiredTime.Year, desiredTime.Month, desiredTime.Day, desiredTime.Hour, 0, 0);
        
        var table = _layout.FindAvailableTable(partySize, desiredTime) 
            ?? throw new InvalidOperationException("No available table found");
        
        var reservation = new Reservation(partyName, partySize, desiredTime, table);
        table.AddReservation(reservation);
        _reservations.Add(reservation);
        return reservation;
    }

    /// <summary>
    /// Removes an existing reservation.
    /// </summary>
    public void RemoveReservation(string partyName, int partySize, DateTime reservationTime)
    {
        foreach (var reservation in _reservations.ToList())
        {
            if (reservation.Time == reservationTime &&
                reservation.PartySize == partySize &&
                reservation.PartyName == partyName)
            {
                reservation.AssignedTable.RemoveReservation(reservationTime);
                _reservations.Remove(reservation);
                return;
            }
        }
    }

    /// <summary>
    /// Returns all current reservations.
    /// </summary>
    public IReadOnlySet<Reservation> Reservations => _reservations;
}
