namespace OodInterview.Elevator;

/// <summary>
/// Handles elevator call buttons on a floor.
/// Observable Subject in the Observer pattern.
/// </summary>
public class HallwayButtonPanel
{
    private readonly int _floor;
    private readonly List<IElevatorObserver> _observers;

    /// <summary>
    /// Creates a button panel for the specified floor.
    /// </summary>
    public HallwayButtonPanel(int floor)
    {
        _floor = floor;
        _observers = [];
    }

    /// <summary>
    /// Simulates pressing the elevator button.
    /// </summary>
    public void PressButton(Direction direction)
    {
        NotifyObservers(direction);
    }

    /// <summary>
    /// Adds an observer to be notified of button presses.
    /// </summary>
    public void AddObserver(IElevatorObserver observer)
    {
        _observers.Add(observer);
    }

    /// <summary>
    /// Notifies all observers of a button press.
    /// </summary>
    private void NotifyObservers(Direction direction)
    {
        foreach (var observer in _observers)
        {
            observer.Update(_floor, direction);
        }
    }
}
