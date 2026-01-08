# Elevator System - OOD Interview

A .NET Core 10 implementation of an elevator control system, ported from the ByteByteGoHq/ood-interview Java repository.

## Overview

This project demonstrates object-oriented design principles through an elevator system that manages multiple elevator cars in a building. The system handles elevator requests, movement, and dispatching using different strategies.

## Project Structure

```
OodInterview.Elevator/
├── ElevatorSystem.cs              # Main system controller
├── Components/
│   ├── Direction.cs               # Elevator direction enum
│   ├── ElevatorCar.cs             # Core elevator functionality
│   ├── ElevatorStatus.cs          # Status representation
│   └── HallwayButtonPanel.cs      # Floor button panel (Observable)
├── Dispatch/
│   ├── IDispatchingStrategy.cs    # Strategy interface
│   ├── ElevatorDispatch.cs        # Dispatch controller
│   ├── ElevatorDispatchController.cs # Observer implementation
│   ├── FirstComeFirstServeStrategy.cs # FCFS strategy
│   └── ShortestSeekTimeFirstStrategy.cs # SSTF strategy
└── Observer/
    └── IElevatorObserver.cs       # Observer interface
```

## Design Patterns

### 1. Strategy Pattern

The system uses the Strategy pattern for elevator dispatching, allowing different algorithms to be swapped at runtime:

```csharp
public interface IDispatchingStrategy
{
    ElevatorCar? SelectElevator(IReadOnlyList<ElevatorCar> elevators, int floor, Direction direction);
}
```

**Implementations:**

1. **First Come First Serve (FCFS)**: Assigns requests to the first available elevator
   ```csharp
   public class FirstComeFirstServeStrategy : IDispatchingStrategy
   {
       public ElevatorCar? SelectElevator(IReadOnlyList<ElevatorCar> elevators, int floor, Direction direction)
       {
           foreach (var elevator in elevators)
           {
               if (elevator.IsIdle || elevator.CurrentDirection == direction)
                   return elevator;
           }
           return elevators[Random.Shared.Next(elevators.Count)];
       }
   }
   ```

2. **Shortest Seek Time First (SSTF)**: Optimizes by selecting the closest elevator
   ```csharp
   public class ShortestSeekTimeFirstStrategy : IDispatchingStrategy
   {
       public ElevatorCar? SelectElevator(IReadOnlyList<ElevatorCar> elevators, int floor, Direction direction)
       {
           ElevatorCar? bestElevator = null;
           int shortestDistance = int.MaxValue;

           foreach (var elevator in elevators)
           {
               int distance = Math.Abs(elevator.CurrentFloor - floor);
               if ((elevator.IsIdle || elevator.CurrentDirection == direction) 
                   && distance < shortestDistance)
               {
                   bestElevator = elevator;
                   shortestDistance = distance;
               }
           }
           return bestElevator;
       }
   }
   ```

### 2. Observer Pattern

The hallway button panels use the Observer pattern to notify the system of elevator requests:

```csharp
// Observer Interface
public interface IElevatorObserver
{
    void Update(int floor, Direction direction);
}

// Observable Subject
public class HallwayButtonPanel
{
    private readonly List<IElevatorObserver> _observers;
    
    public void PressButton(Direction direction)
    {
        NotifyObservers(direction);
    }
    
    public void AddObserver(IElevatorObserver observer)
    {
        _observers.Add(observer);
    }
    
    private void NotifyObservers(Direction direction)
    {
        foreach (var observer in _observers)
            observer.Update(_floor, direction);
    }
}
```

### 3. Immutable Status Objects

The `ElevatorStatus` class is immutable, providing a snapshot of elevator state:

```csharp
public class ElevatorStatus
{
    public int CurrentFloor { get; }
    public Direction CurrentDirection { get; }

    public ElevatorStatus(int currentFloor, Direction currentDirection)
    {
        CurrentFloor = currentFloor;
        CurrentDirection = currentDirection;
    }
}
```

## Key Classes

### ElevatorSystem

The main controller that orchestrates:
- Multiple elevator cars
- Dispatching strategy
- Floor requests from hallways and inside elevators

```csharp
var car = new ElevatorCar(1);
var elevatorSystem = new ElevatorSystem([car], new FirstComeFirstServeStrategy());

// Request an elevator from floor 3 going up
elevatorSystem.RequestElevator(3, Direction.Up);

// Select a destination floor from inside the elevator
elevatorSystem.SelectFloor(car, 8);
```

### ElevatorCar

Manages individual elevator behavior:
- Current floor and direction tracking
- Floor request queue
- Step-by-step and direct movement

```csharp
var car = new ElevatorCar(1); // Start at floor 1

car.AddFloorRequest(5);       // Add floor 5 to queue
car.MoveOneStep();            // Move one floor toward destination
car.MoveUntilNextFloor();     // Move directly to next destination
car.IsAtDestination;          // Check if at destination
car.NextDestination();        // Clear current destination, move to next
```

### ElevatorDispatch

Uses the configured strategy to dispatch elevators:

```csharp
public class ElevatorDispatch
{
    private readonly IDispatchingStrategy _strategy;

    public void DispatchElevatorCar(int floor, Direction direction, IReadOnlyList<ElevatorCar> elevators)
    {
        var selectedElevator = _strategy.SelectElevator(elevators, floor, direction);
        selectedElevator?.AddFloorRequest(floor);
    }
}
```

## System Features

1. **Basic Elevator Operations**
   - Move up and down
   - Stop at requested floors
   - Track current floor and direction
   - Handle floor requests

2. **Dispatching Strategies**
   - First Come First Serve: Assigns requests to elevators in order
   - Shortest Seek Time First: Optimizes elevator selection based on distance

3. **Status Monitoring**
   - Current floor tracking
   - Direction status (UP, DOWN, IDLE)
   - Destination queue management

## Test Cases

The test suite covers:

1. **TestElevatorInitialState** - Verifies elevator starts idle at specified floor
2. **TestElevatorRequestAndMovement** - Tests step-by-step movement to destination
3. **TestMultipleFloorRequests** - Tests handling multiple requests in queue
4. **TestShortestSeekTimeFirstStrategy** - Verifies SSTF selects closest elevator
5. **TestSelectFloorFromInsideElevator** - Tests destination selection from inside car
6. **TestHallwayButtonPanel** - Tests Observer pattern with button panels

## Running the Project

```bash
# Build the project
dotnet build src/OodInterview.Elevator

# Run tests
dotnet test tests/OodInterview.Elevator.Tests

# Run with verbose output
dotnet test tests/OodInterview.Elevator.Tests --verbosity normal
```

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `LinkedList<Integer>` | `Queue<int>` |
| `Queue.peek()` | `Queue.Peek()` |
| `Queue.poll()` | `Queue.Dequeue()` |
| `Queue.offer()` | `Queue.Enqueue()` |
| `Math.random()` | `Random.Shared.Next()` |
| Interface `DispatchingStrategy` | Interface `IDispatchingStrategy` |
| Interface `ElevatorObserver` | Interface `IElevatorObserver` |

## Comparison: FCFS vs SSTF

| Scenario | FCFS | SSTF |
|----------|------|------|
| Simple buildings | ✅ Fair, predictable | ⚠️ May starve distant requests |
| High traffic | ⚠️ Longer wait times | ✅ Better throughput |
| Implementation | Simple | More complex |
| Starvation risk | Low | Higher |

## Future Enhancements

1. **SCAN Algorithm**: Elevator moves in one direction until no more requests
2. **LOOK Algorithm**: Similar to SCAN but reverses at last request
3. **Priority Requests**: VIP or emergency floor requests
4. **Load Balancing**: Distribute requests evenly across elevators
5. **Zone Dispatching**: Assign elevators to specific floor ranges
