namespace OodInterview.Elevator;

/// <summary>
/// Represents the current status of an elevator.
/// </summary>
public class ElevatorStatus
{
    /// <summary>
    /// The current floor of the elevator.
    /// </summary>
    public int CurrentFloor { get; }

    /// <summary>
    /// The current direction of the elevator.
    /// </summary>
    public Direction CurrentDirection { get; }

    /// <summary>
    /// Creates a new elevator status.
    /// </summary>
    public ElevatorStatus(int currentFloor, Direction currentDirection)
    {
        CurrentFloor = currentFloor;
        CurrentDirection = currentDirection;
    }
}
