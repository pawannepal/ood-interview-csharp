namespace OodInterview.Elevator;

/// <summary>
/// Main elevator system controller that manages multiple elevators.
/// </summary>
public class ElevatorSystem
{
    private readonly List<ElevatorCar> _elevators;
    private readonly ElevatorDispatch _dispatchController;

    /// <summary>
    /// Creates a new elevator system with the given elevators and dispatching strategy.
    /// </summary>
    public ElevatorSystem(IEnumerable<ElevatorCar> elevators, IDispatchingStrategy strategy)
    {
        _elevators = elevators.ToList();
        _dispatchController = new ElevatorDispatch(strategy);
    }

    /// <summary>
    /// Gets the status of all elevators in the system.
    /// </summary>
    public List<ElevatorStatus> GetAllElevatorStatuses()
    {
        var statuses = new List<ElevatorStatus>();
        foreach (var elevator in _elevators)
        {
            statuses.Add(elevator.Status);
        }
        return statuses;
    }

    /// <summary>
    /// Requests an elevator from a floor going in a direction.
    /// </summary>
    public void RequestElevator(int currentFloor, Direction direction)
    {
        _dispatchController.DispatchElevatorCar(currentFloor, direction, _elevators);
    }

    /// <summary>
    /// Selects a destination floor from inside an elevator car.
    /// </summary>
    public void SelectFloor(ElevatorCar car, int destinationFloor)
    {
        // Selecting the floor from within the elevator is directly handled by the elevator car
        car.AddFloorRequest(destinationFloor);
    }
}
