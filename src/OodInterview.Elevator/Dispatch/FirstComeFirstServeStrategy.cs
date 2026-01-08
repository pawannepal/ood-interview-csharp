namespace OodInterview.Elevator;

/// <summary>
/// First Come First Serve dispatching strategy.
/// Assigns requests to the first available elevator.
/// </summary>
public class FirstComeFirstServeStrategy : IDispatchingStrategy
{
    /// <summary>
    /// Selects the first idle or same-direction elevator.
    /// </summary>
    public ElevatorCar? SelectElevator(IReadOnlyList<ElevatorCar> elevators, int floor, Direction direction)
    {
        foreach (var elevator in elevators)
        {
            if (elevator.IsIdle || elevator.CurrentDirection == direction)
            {
                return elevator;
            }
        }

        // If no suitable elevator found, return a random one
        if (elevators.Count > 0)
        {
            return elevators[Random.Shared.Next(elevators.Count)];
        }

        return null;
    }
}
