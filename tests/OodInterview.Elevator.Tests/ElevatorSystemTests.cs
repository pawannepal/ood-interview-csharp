using OodInterview.Elevator;

namespace OodInterview.Elevator.Tests;

public class ElevatorSystemTests
{
    [Fact]
    public void TestElevatorInitialState()
    {
        // Create an elevator car starting at floor 1
        var car = new ElevatorCar(1);
        var elevatorSystem = new ElevatorSystem([car], new FirstComeFirstServeStrategy());

        // Test that the elevator is idle
        var elevators = elevatorSystem.GetAllElevatorStatuses();
        Assert.Equal(1, elevators[0].CurrentFloor);
        Assert.Equal(Direction.Idle, elevators[0].CurrentDirection);
    }

    [Fact]
    public void TestElevatorRequestAndMovement()
    {
        // Set up
        var car = new ElevatorCar(1);
        var elevatorSystem = new ElevatorSystem([car], new FirstComeFirstServeStrategy());

        // Test action to hail the elevator from floor 3
        elevatorSystem.RequestElevator(3, Direction.Up);

        // Test that the elevator is moving
        var elevators = elevatorSystem.GetAllElevatorStatuses();
        Assert.Equal(1, elevators[0].CurrentFloor);
        Assert.Equal(Direction.Up, elevators[0].CurrentDirection);
        Assert.False(car.IsAtDestination);

        // Test the elevator car's queue is to go to floor 3
        car.MoveOneStep();
        elevators = elevatorSystem.GetAllElevatorStatuses();
        Assert.Equal(2, elevators[0].CurrentFloor);
        Assert.Equal(Direction.Up, elevators[0].CurrentDirection);
        Assert.False(car.IsAtDestination);

        // Move to floor 3
        car.MoveOneStep();
        elevators = elevatorSystem.GetAllElevatorStatuses();
        Assert.Equal(3, elevators[0].CurrentFloor);
        Assert.Equal(Direction.Up, elevators[0].CurrentDirection);
        Assert.True(car.IsAtDestination);

        // Arrive at destination
        car.MoveOneStep();
        elevators = elevatorSystem.GetAllElevatorStatuses();
        Assert.Equal(3, elevators[0].CurrentFloor);
        Assert.Equal(Direction.Idle, elevators[0].CurrentDirection);
        Assert.False(car.IsAtDestination);
    }

    [Fact]
    public void TestMultipleFloorRequests()
    {
        // Set up
        var car = new ElevatorCar(1);
        var elevatorSystem = new ElevatorSystem([car], new FirstComeFirstServeStrategy());

        // First get to floor 3
        elevatorSystem.RequestElevator(3, Direction.Up);
        car.MoveUntilNextFloor();
        car.MoveOneStep(); // Complete the destination

        // Now test the moveUntilNextFloor method, hail the elevator in both floor 8 and floor 6
        elevatorSystem.RequestElevator(8, Direction.Down);
        elevatorSystem.RequestElevator(6, Direction.Down);

        // Test that the elevator is moving to floor 8
        car.MoveUntilNextFloor();
        var elevators = elevatorSystem.GetAllElevatorStatuses();
        Assert.Equal(8, elevators[0].CurrentFloor);
        Assert.Equal(Direction.Up, elevators[0].CurrentDirection);
        Assert.True(car.IsAtDestination);

        // Test that the elevator is moving to floor 6
        car.NextDestination();
        car.MoveUntilNextFloor();
        elevators = elevatorSystem.GetAllElevatorStatuses();
        Assert.Equal(6, elevators[0].CurrentFloor);
        Assert.Equal(Direction.Down, elevators[0].CurrentDirection);
        Assert.True(car.IsAtDestination);
    }

    [Fact]
    public void TestShortestSeekTimeFirstStrategy()
    {
        // Create two elevators at different floors
        var car1 = new ElevatorCar(1);
        var car2 = new ElevatorCar(5);
        var elevatorSystem = new ElevatorSystem([car1, car2], new ShortestSeekTimeFirstStrategy());

        // Request from floor 4 - car2 (at floor 5) should be selected
        elevatorSystem.RequestElevator(4, Direction.Down);

        // car2 should have the request (it's closer)
        Assert.True(car1.IsIdle);
        Assert.False(car2.IsIdle);
        Assert.Equal(Direction.Down, car2.CurrentDirection);
    }

    [Fact]
    public void TestSelectFloorFromInsideElevator()
    {
        var car = new ElevatorCar(1);
        var elevatorSystem = new ElevatorSystem([car], new FirstComeFirstServeStrategy());

        // Select floor 5 from inside the elevator
        elevatorSystem.SelectFloor(car, 5);

        Assert.False(car.IsIdle);
        Assert.Equal(Direction.Up, car.CurrentDirection);

        // Move to destination
        car.MoveUntilNextFloor();
        Assert.Equal(5, car.CurrentFloor);
        Assert.True(car.IsAtDestination);
    }

    [Fact]
    public void TestHallwayButtonPanel()
    {
        // Create a test observer
        var testObserver = new TestElevatorObserver();
        var buttonPanel = new HallwayButtonPanel(5);
        
        buttonPanel.AddObserver(testObserver);
        buttonPanel.PressButton(Direction.Up);

        Assert.Equal(5, testObserver.LastFloor);
        Assert.Equal(Direction.Up, testObserver.LastDirection);
    }

    /// <summary>
    /// Test observer for verifying button panel notifications.
    /// </summary>
    private class TestElevatorObserver : IElevatorObserver
    {
        public int LastFloor { get; private set; }
        public Direction LastDirection { get; private set; }

        public void Update(int floor, Direction direction)
        {
            LastFloor = floor;
            LastDirection = direction;
        }
    }
}
