# Parking Lot System

This is a .NET Core 10 implementation of a parking lot system that manages vehicle parking, spot allocation, and fare calculation.

## Project Structure

```
OodInterview.ParkingLot/
├── ParkingLotSystem.cs         # Main parking lot facade
├── Vehicle/
│   ├── VehicleSize.cs         # Enum for vehicle sizes
│   ├── IVehicle.cs            # Vehicle interface
│   ├── Car.cs                 # Medium-sized vehicle
│   ├── Motorcycle.cs          # Small-sized vehicle
│   └── Truck.cs               # Large-sized vehicle
├── Spot/
│   ├── IParkingSpot.cs        # Parking spot interface
│   ├── CompactSpot.cs         # For small vehicles
│   ├── RegularSpot.cs         # For medium vehicles
│   ├── OversizedSpot.cs       # For large vehicles
│   ├── HandicappedSpot.cs     # Handicapped parking
│   └── ParkingManager.cs      # Manages spot allocation
└── Fare/
    ├── Ticket.cs              # Parking ticket
    ├── IFareStrategy.cs       # Fare strategy interface
    ├── BaseFareStrategy.cs    # Base fare calculation
    ├── PeakHoursFareStrategy.cs # Peak hours multiplier
    └── FareCalculator.cs      # Combines strategies
```

## Design Patterns Used

### 1. Strategy Pattern

The Strategy pattern is implemented in two places:

#### Fare Calculation Strategies
Different strategies can be applied to calculate parking fees:

```csharp
public interface IFareStrategy
{
    decimal CalculateFare(Ticket ticket, decimal inputFare);
}

public class BaseFareStrategy : IFareStrategy
{
    public decimal CalculateFare(Ticket ticket, decimal inputFare)
    {
        var rate = ticket.Vehicle.Size switch
        {
            VehicleSize.Medium => 2.0m,
            VehicleSize.Large => 3.0m,
            _ => 1.0m
        };
        return inputFare + (rate * ticket.CalculateParkingDuration());
    }
}

public class PeakHoursFareStrategy : IFareStrategy
{
    public decimal CalculateFare(Ticket ticket, decimal inputFare)
    {
        if (IsPeakHours(ticket.EntryTime))
            return inputFare * 1.5m;
        return inputFare;
    }
}
```

**Benefits:**
- Different fare strategies can be combined
- Easy to add new pricing rules (weekend rates, holiday rates)
- Strategies are applied in sequence (Chain of Responsibility variant)

### 2. Polymorphism (Vehicle & Spot Types)

The system uses interfaces for vehicles and parking spots:

```csharp
public interface IVehicle
{
    string LicensePlate { get; }
    VehicleSize Size { get; }
}

public interface IParkingSpot
{
    int SpotNumber { get; }
    VehicleSize Size { get; }
    bool IsAvailable { get; }
    void Occupy(IVehicle vehicle);
    void Vacate();
}
```

**Vehicle Types:**
- `Motorcycle` - Small size
- `Car` - Medium size
- `Truck` - Large size

**Spot Types:**
- `CompactSpot` - For small vehicles
- `RegularSpot` - For medium vehicles
- `OversizedSpot` - For large vehicles
- `HandicappedSpot` - Accessible parking

### 3. Facade Pattern

The `ParkingLotSystem` provides a simplified interface:

```csharp
public class ParkingLotSystem
{
    public Ticket? EnterVehicle(IVehicle vehicle) { ... }
    public decimal? LeaveVehicle(Ticket ticket) { ... }
    public IParkingSpot? FindVehicleSpot(IVehicle vehicle) { ... }
}
```

## Key Features

### Smart Spot Allocation
Vehicles can park in spots of their size or larger:
- Motorcycles can park in compact, regular, or oversized spots
- Cars can park in regular or oversized spots
- Trucks can only park in oversized spots

### Duration-Based Pricing
- Small vehicles: $1.00 per minute
- Medium vehicles: $2.00 per minute
- Large vehicles: $3.00 per minute

### Peak Hours Surcharge
50% surcharge during peak hours (7-10 AM and 4-7 PM)

## Usage Example

```csharp
// Set up parking spots
var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
{
    [VehicleSize.Small] = new List<IParkingSpot> { new CompactSpot(1) },
    [VehicleSize.Medium] = new List<IParkingSpot> { new RegularSpot(2), new RegularSpot(3) },
    [VehicleSize.Large] = new List<IParkingSpot> { new OversizedSpot(4) }
};

// Initialize managers
var parkingManager = new ParkingManager(availableSpots);
var fareCalculator = new FareCalculator(new List<IFareStrategy>
{
    new BaseFareStrategy(),
    new PeakHoursFareStrategy()
});

var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

// Vehicle enters
var car = new Car("ABC123");
var ticket = parkingLot.EnterVehicle(car);
Console.WriteLine($"Ticket: {ticket.TicketId}");
Console.WriteLine($"Spot: {ticket.ParkingSpot.SpotNumber}");

// Find parked vehicle
var spot = parkingLot.FindVehicleSpot(car);
Console.WriteLine($"Vehicle parked at spot: {spot.SpotNumber}");

// Vehicle exits
var fare = parkingLot.LeaveVehicle(ticket);
Console.WriteLine($"Total fare: ${fare:F2}");
```

## Running the Tests

```bash
# From repository root
dotnet test tests/OodInterview.ParkingLot.Tests

# Or run specific tests
dotnet test tests/OodInterview.ParkingLot.Tests --filter "FullyQualifiedName~ParkingLotSystemTests"
```

## Test Coverage

The test suite covers:
- Complete vehicle journey (enter, park, exit)
- Different vehicle types (motorcycle, car, truck)
- No spot available scenario
- Spot becomes available after exit
- Small vehicles in larger spots
- Fare calculation
- Multiple vehicle sizes simultaneously

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `BigDecimal` | `decimal` |
| `LocalDateTime` | `DateTime` |
| `Duration` | `TimeSpan` |
| `HashMap<K, V>` | `Dictionary<K, V>` |
| `ArrayList<T>` | `List<T>` |
| `Vehicle` interface | `IVehicle` interface |
| `ParkingSpot` interface | `IParkingSpot` interface |
| `FareStrategy` interface | `IFareStrategy` interface |
| `parkVehicle()` | `ParkVehicle()` |
| `unparkVehicle()` | `UnparkVehicle()` |
