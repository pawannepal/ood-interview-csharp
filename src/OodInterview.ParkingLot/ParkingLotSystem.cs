using OodInterview.ParkingLot.Fare;
using OodInterview.ParkingLot.Spot;
using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot;

/// <summary>
/// Manages the parking lot operations including vehicle entry and exit.
/// </summary>
public class ParkingLotSystem
{
    private readonly ParkingManager _parkingManager;
    private readonly FareCalculator _fareCalculator;
    private long _ticketCounter;

    public ParkingLotSystem(ParkingManager parkingManager, FareCalculator fareCalculator)
    {
        _parkingManager = parkingManager;
        _fareCalculator = fareCalculator;
    }

    /// <summary>
    /// Handles vehicle entry into the parking lot.
    /// </summary>
    /// <param name="vehicle">The vehicle entering.</param>
    /// <returns>A parking ticket, or null if no spot available.</returns>
    public Ticket? EnterVehicle(IVehicle vehicle)
    {
        var spot = _parkingManager.ParkVehicle(vehicle);
        if (spot != null)
        {
            var ticket = new Ticket(GenerateTicketId(), vehicle, spot, DateTime.Now);
            return ticket;
        }
        return null;
    }

    /// <summary>
    /// Handles vehicle exit from the parking lot.
    /// </summary>
    /// <param name="ticket">The parking ticket.</param>
    /// <returns>The calculated fare, or null if invalid ticket.</returns>
    public decimal? LeaveVehicle(Ticket ticket)
    {
        if (ticket.ExitTime == null)
        {
            ticket.ExitTime = DateTime.Now;
            _parkingManager.UnparkVehicle(ticket.Vehicle);
            var fare = _fareCalculator.CalculateFare(ticket);
            return fare;
        }
        return null;
    }

    /// <summary>
    /// Finds the spot where a vehicle is parked.
    /// </summary>
    /// <param name="vehicle">The vehicle to find.</param>
    /// <returns>The parking spot, or null if not found.</returns>
    public IParkingSpot? FindVehicleSpot(IVehicle vehicle)
    {
        return _parkingManager.FindVehicleSpot(vehicle);
    }

    private string GenerateTicketId()
    {
        return $"TICKET-{Interlocked.Increment(ref _ticketCounter)}";
    }
}
