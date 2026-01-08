using OodInterview.ParkingLot.Spot;
using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Fare;

/// <summary>
/// Represents a parking ticket.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Creates a new parking ticket.
    /// </summary>
    /// <param name="ticketId">Unique ticket identifier.</param>
    /// <param name="vehicle">The vehicle associated with this ticket.</param>
    /// <param name="parkingSpot">The parking spot assigned.</param>
    /// <param name="entryTime">The time the vehicle entered.</param>
    public Ticket(string ticketId, IVehicle vehicle, IParkingSpot parkingSpot, DateTime entryTime)
    {
        TicketId = ticketId;
        Vehicle = vehicle;
        ParkingSpot = parkingSpot;
        EntryTime = entryTime;
    }

    /// <summary>
    /// Gets the ticket ID.
    /// </summary>
    public string TicketId { get; }

    /// <summary>
    /// Gets the vehicle associated with this ticket.
    /// </summary>
    public IVehicle Vehicle { get; }

    /// <summary>
    /// Gets the parking spot assigned.
    /// </summary>
    public IParkingSpot ParkingSpot { get; }

    /// <summary>
    /// Gets the entry time.
    /// </summary>
    public DateTime EntryTime { get; }

    /// <summary>
    /// Gets or sets the exit time.
    /// </summary>
    public DateTime? ExitTime { get; set; }

    /// <summary>
    /// Calculates the parking duration in minutes.
    /// </summary>
    /// <returns>The parking duration in minutes.</returns>
    public decimal CalculateParkingDuration()
    {
        var exitTime = ExitTime ?? DateTime.Now;
        return (decimal)(exitTime - EntryTime).TotalMinutes;
    }
}
