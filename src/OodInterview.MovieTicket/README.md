# Movie Ticket Booking System

This is a .NET Core 10 implementation of a movie ticket booking system that allows users to manage cinemas, movies, screenings, and ticket bookings.

## Project Structure

```
OodInterview.MovieTicket/
├── MovieBookingSystem.cs       # Main booking system facade
├── Location/
│   ├── Cinema.cs              # Cinema with multiple rooms
│   ├── Room.cs                # Room with seating layout
│   ├── Layout.cs              # Seating layout with position-based access
│   └── Seat.cs                # Individual seat with pricing strategy
├── Showing/
│   ├── Movie.cs               # Movie details
│   └── Screening.cs           # Scheduled movie showing
├── Ticket/
│   ├── Ticket.cs              # Booking ticket
│   ├── Order.cs               # Order with multiple tickets
│   └── ScreeningManager.cs    # Manages screenings and tickets
└── Rate/
    ├── IPricingStrategy.cs    # Interface for seat pricing
    ├── NormalRate.cs          # Normal pricing strategy
    ├── PremiumRate.cs         # Premium pricing strategy
    └── VipRate.cs             # VIP pricing strategy
```

## Design Patterns Used

### 1. Strategy Pattern

The Strategy pattern is implemented through the pricing system, allowing different pricing strategies for seats.

```csharp
public interface IPricingStrategy
{
    decimal Price { get; }
}

public class NormalRate : IPricingStrategy
{
    public NormalRate(decimal price) => Price = price;
    public decimal Price { get; }
}

public class PremiumRate : IPricingStrategy
{
    public PremiumRate(decimal price) => Price = price;
    public decimal Price { get; }
}

public class VipRate : IPricingStrategy
{
    public VipRate(decimal price) => Price = price;
    public decimal Price { get; }
}
```

**Benefits:**
- Each seat can have its own pricing strategy
- Easy to add new pricing tiers (e.g., student rates, senior rates)
- Prices can be changed at runtime without modifying seat structure
- Open/Closed Principle: system is open for extension but closed for modification

### 2. Facade Pattern

The `MovieBookingSystem` class provides a simplified interface to the complex subsystem of cinemas, movies, screenings, and tickets.

```csharp
public class MovieBookingSystem
{
    private readonly ScreeningManager _screeningManager = new();

    public void AddMovie(Movie movie) { ... }
    public void AddCinema(Cinema cinema) { ... }
    public void AddScreening(Movie movie, Screening screening) { ... }
    public void BookTicket(Screening screening, Seat seat) { ... }
    public List<Seat> GetAvailableSeats(Screening screening) { ... }
}
```

**Benefits:**
- Provides a simple, high-level API for clients
- Hides the complexity of managing screenings and tickets
- Single point of entry for all booking operations

### 3. Composition Pattern

The system uses composition to build complex structures:

```
Cinema
  └── Room (1..*)
        └── Layout (1)
              └── Seat (m×n)
                    └── IPricingStrategy (1)
```

## Key Classes

### Cinema & Room
- A `Cinema` has a name, location, and contains multiple `Room` objects
- Each `Room` has a room number and a `Layout`

### Layout
- Represents the seating arrangement in a room
- Provides dual access patterns:
  - By seat number (e.g., "0-0", "1-2")
  - By position (row, column)

### Seat
- Represents an individual seat
- Has a seat number and an optional pricing strategy
- Pricing can be set or changed dynamically

### Screening
- Links a `Movie` to a `Room` at a specific time
- Includes start and end times

### ScreeningManager
- Manages relationships between movies, screenings, and tickets
- Tracks which seats are booked for each screening
- Calculates available seats

## Usage Example

```csharp
// Create a booking system
var bookingSystem = new MovieBookingSystem();

// Create a room with 10x10 seats
var room = new Room("1", new Layout(10, 10));

// Set pricing for all seats
for (var i = 0; i < 10; i++)
{
    for (var j = 0; j < 10; j++)
    {
        room.Layout.GetSeatByPosition(i, j)!.PricingStrategy = new NormalRate(10.00m);
    }
}

// Create and add cinema
var cinema = new Cinema("Grand Cinema", "Downtown");
cinema.AddRoom(room);
bookingSystem.AddCinema(cinema);

// Create movie and screening
var movie = new Movie("The Matrix", "Sci-Fi", 136);
var screening = new Screening(
    movie, 
    room, 
    DateTime.Now, 
    DateTime.Now.AddMinutes(136)
);

bookingSystem.AddMovie(movie);
bookingSystem.AddScreening(movie, screening);

// Check available seats
var availableSeats = bookingSystem.GetAvailableSeats(screening);
Console.WriteLine($"Available seats: {availableSeats.Count}");

// Book a ticket
bookingSystem.BookTicket(screening, room.Layout.GetSeatByPosition(0, 0)!);

// Verify booking
var tickets = bookingSystem.GetTicketsForScreening(screening);
Console.WriteLine($"Ticket price: ${tickets[0].Price}");
```

## Running the Tests

```bash
# From repository root
dotnet test tests/OodInterview.MovieTicket.Tests

# Or run specific tests
dotnet test tests/OodInterview.MovieTicket.Tests --filter "FullyQualifiedName~MovieBookingSystemTests"
```

## Test Coverage

The test suite covers:
- Basic browse and buy flow
- Multiple room bookings with different pricing
- VIP seat pricing
- Available seats tracking after bookings
- Cinema with multiple rooms
- Seat access by number and position
- Movie details

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `BigDecimal` | `decimal` |
| `LocalDateTime` | `DateTime` |
| `Duration` | `TimeSpan` |
| `HashMap<K, V>` | `Dictionary<K, V>` |
| `ArrayList<T>` | `List<T>` |
| `getAvailableSeats()` | `GetAvailableSeats()` |
| `computeIfAbsent()` | `TryGetValue()` with initialization |
| `getOrDefault()` | `GetValueOrDefault()` |
