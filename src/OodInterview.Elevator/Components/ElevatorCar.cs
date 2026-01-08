namespace OodInterview.Elevator;

/// <summary>
/// Represents an elevator car with movement and floor request handling.
/// </summary>
public class ElevatorCar
{
    private ElevatorStatus _status;
    private readonly Queue<int> _targetFloors;

    /// <summary>
    /// Creates a new elevator car at the specified starting floor.
    /// </summary>
    public ElevatorCar(int startingFloor)
    {
        _status = new ElevatorStatus(startingFloor, Direction.Idle);
        _targetFloors = new Queue<int>();
    }

    /// <summary>
    /// Gets the current status of the elevator.
    /// </summary>
    public ElevatorStatus Status => _status;

    /// <summary>
    /// Adds a floor request to the elevator's queue.
    /// </summary>
    public void AddFloorRequest(int floor)
    {
        if (!_targetFloors.Contains(floor))
        {
            _targetFloors.Enqueue(floor);
            UpdateDirection(floor);
        }
    }

    /// <summary>
    /// Gets the current floor of the elevator.
    /// </summary>
    public int CurrentFloor => _status.CurrentFloor;

    /// <summary>
    /// Gets the current direction of the elevator.
    /// </summary>
    public Direction CurrentDirection => _status.CurrentDirection;

    /// <summary>
    /// Returns true if the elevator has no pending floor requests.
    /// </summary>
    public bool IsIdle => _targetFloors.Count == 0;

    /// <summary>
    /// Updates the direction based on the target floor.
    /// </summary>
    private void UpdateDirection(int targetFloor)
    {
        if (_status.CurrentFloor < targetFloor)
        {
            _status = new ElevatorStatus(_status.CurrentFloor, Direction.Up);
        }
        else if (_status.CurrentFloor > targetFloor)
        {
            _status = new ElevatorStatus(_status.CurrentFloor, Direction.Down);
        }
    }

    /// <summary>
    /// Moves the elevator one floor toward its next destination.
    /// </summary>
    public void MoveOneStep()
    {
        if (_targetFloors.Count == 0) return;

        int nextFloor = _targetFloors.Peek();
        if (_status.CurrentFloor < nextFloor)
        {
            _status = new ElevatorStatus(_status.CurrentFloor + 1, Direction.Up);
        }
        else if (_status.CurrentFloor > nextFloor)
        {
            _status = new ElevatorStatus(_status.CurrentFloor - 1, Direction.Down);
        }
        else
        {
            _targetFloors.Dequeue();
            if (_targetFloors.Count == 0)
            {
                _status = new ElevatorStatus(_status.CurrentFloor, Direction.Idle);
            }
        }
    }

    /// <summary>
    /// Returns true if the elevator is at its next destination floor.
    /// </summary>
    public bool IsAtDestination =>
        _targetFloors.Count > 0 && _status.CurrentFloor == _targetFloors.Peek();

    /// <summary>
    /// Removes the current destination from the queue if at destination.
    /// </summary>
    public void NextDestination()
    {
        if (IsAtDestination)
        {
            _targetFloors.Dequeue();
        }
    }

    /// <summary>
    /// Moves the elevator directly to the next destination floor.
    /// </summary>
    public void MoveUntilNextFloor()
    {
        if (_targetFloors.Count > 0)
        {
            int nextFloor = _targetFloors.Peek();
            while (_status.CurrentFloor != nextFloor)
            {
                MoveOneStep();
            }
        }
    }
}
