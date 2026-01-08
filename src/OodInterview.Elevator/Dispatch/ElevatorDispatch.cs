namespace OodInterview.Elevator;

/// <summary>
/// Handles dispatching of elevator cars based on a strategy.
/// </summary>
public class ElevatorDispatch
{
    private readonly IDispatchingStrategy _strategy;

    /// <summary>
    /// Creates a new elevator dispatch controller with the given strategy.
    /// </summary>
    public ElevatorDispatch(IDispatchingStrategy strategy)
    {
        _strategy = strategy;
    }

    /// <summary>
    /// Dispatches an elevator to handle a floor request.
    /// </summary>
    public void DispatchElevatorCar(int floor, Direction direction, IReadOnlyList<ElevatorCar> elevators)
    {
        var selectedElevator = _strategy.SelectElevator(elevators, floor, direction);
        selectedElevator?.AddFloorRequest(floor);
    }
}
