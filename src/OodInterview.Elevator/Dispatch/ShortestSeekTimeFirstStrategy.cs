namespace OodInterview.Elevator;

/// <summary>
/// Shortest Seek Time First dispatching strategy.
/// Selects the elevator with the shortest distance to the requested floor.
/// </summary>
public class ShortestSeekTimeFirstStrategy : IDispatchingStrategy
{
    /// <summary>
    /// Selects the elevator with the shortest distance to the floor.
    /// </summary>
    public ElevatorCar? SelectElevator(IReadOnlyList<ElevatorCar> elevators, int floor, Direction direction)
    {
        ElevatorCar? bestElevator = null;
        int shortestDistance = int.MaxValue;

        foreach (var elevator in elevators)
        {
            int distance = Math.Abs(elevator.CurrentFloor - floor);
            if ((elevator.IsIdle || elevator.CurrentDirection == direction) && distance < shortestDistance)
            {
                bestElevator = elevator;
                shortestDistance = distance;
            }
        }

        return bestElevator;
    }
}
