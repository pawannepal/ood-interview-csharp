namespace OodInterview.Elevator;

/// <summary>
/// Strategy interface for selecting which elevator to dispatch.
/// </summary>
public interface IDispatchingStrategy
{
    /// <summary>
    /// Selects the best elevator to handle a request.
    /// </summary>
    ElevatorCar? SelectElevator(IReadOnlyList<ElevatorCar> elevators, int floor, Direction direction);
}
