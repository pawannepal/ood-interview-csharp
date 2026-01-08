namespace OodInterview.Elevator;

/// <summary>
/// Observer interface for elevator events.
/// </summary>
public interface IElevatorObserver
{
    /// <summary>
    /// Called when an elevator is requested from a floor.
    /// </summary>
    void Update(int floor, Direction direction);
}
