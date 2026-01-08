# OodInterview.Restaurant

A restaurant management system that handles reservations, table assignment, menu management, and order processing using the **Command Pattern**.

## Overview

This project simulates a restaurant management system with capabilities for:
- **Reservation Management**: Creating and canceling scheduled and walk-in reservations
- **Table Assignment**: Automatically assigning the smallest available table that fits the party
- **Menu Management**: Managing menu items with categories (Main, Appetizer, Dessert)
- **Order Processing**: Ordering, delivering, and canceling food items using the Command Pattern

## Design Patterns Used

### 1. Command Pattern

The **Command Pattern** is the primary design pattern in this project. It encapsulates order-related requests as objects, allowing for:
- **Decoupling**: Separates the object that invokes the operation from the one that performs it
- **Extensibility**: Easy to add new commands without changing existing code
- **Queueing**: Commands can be queued and executed in sequence

#### Implementation

```csharp
// Command Interface
public interface IOrderCommand
{
    void Execute();
}

// Concrete Commands
public class SendToKitchenCommand : IOrderCommand
{
    private readonly OrderItem _orderItem;
    
    public SendToKitchenCommand(OrderItem orderItem) => _orderItem = orderItem;
    
    public void Execute() => _orderItem.SendToKitchen();
}

public class DeliverCommand : IOrderCommand
{
    private readonly OrderItem _orderItem;
    
    public DeliverCommand(OrderItem orderItem) => _orderItem = orderItem;
    
    public void Execute() => _orderItem.DeliverToCustomer();
}

public class CancelCommand : IOrderCommand
{
    private readonly OrderItem _orderItem;
    
    public CancelCommand(OrderItem orderItem) => _orderItem = orderItem;
    
    public void Execute() => _orderItem.Cancel();
}

// Invoker
public class OrderManager
{
    private readonly List<IOrderCommand> _commandQueue = [];
    
    public void AddCommand(IOrderCommand command) => _commandQueue.Add(command);
    
    public void ExecuteCommands()
    {
        foreach (var command in _commandQueue)
            command.Execute();
        _commandQueue.Clear();
    }
}
```

### 2. Facade Pattern

The `RestaurantSystem` class acts as a **Facade**, providing a simplified interface to the complex subsystems:
- `ReservationManager`: Handles reservation logic
- `Layout`: Manages table collection and availability
- `Menu`: Stores and retrieves menu items
- `OrderManager`: Processes order commands

```csharp
public class RestaurantSystem
{
    public RestaurantSystem(string name, Menu menu, Layout layout)
    {
        Name = name;
        Menu = menu;
        Layout = layout;
        ReservationManager = new ReservationManager(layout);
    }
    
    // Simplified interface for common operations
    public Reservation CreateScheduledReservation(string partyName, int partySize, DateTime time) =>
        ReservationManager.CreateReservation(partyName, partySize, time);
    
    public void OrderItem(Table table, MenuItem item) { /* ... */ }
    public decimal CalculateTableBill(Table table) => table.CalculateBillAmount();
}
```

### 3. State Pattern (Implicit)

The `OrderItem` class uses an implicit state pattern through the `OrderStatus` enum with controlled state transitions:

```
Pending → SentToKitchen → Delivered
    ↓          ↓
Canceled ← ← ← ┘
```

State transition rules:
- `SendToKitchen()`: Only works from `Pending`
- `DeliverToCustomer()`: Only works from `SentToKitchen`
- `Cancel()`: Works from `Pending` or `SentToKitchen`, but not `Delivered`

## Architecture

```
OodInterview.Restaurant/
├── RestaurantSystem.cs          # Main facade
├── Menu/
│   ├── Category.cs              # Menu item categories enum
│   ├── MenuItem.cs              # Menu item entity
│   └── Menu.cs                  # Menu collection manager
├── Table/
│   ├── Table.cs                 # Table entity with orders/reservations
│   └── Layout.cs                # Table layout manager
├── Reservation/
│   ├── OrderStatus.cs           # Order status enum
│   ├── OrderItem.cs             # Ordered item with status tracking
│   ├── Reservation.cs           # Reservation entity
│   └── ReservationManager.cs    # Reservation manager
└── Command/
    ├── IOrderCommand.cs         # Command interface
    ├── SendToKitchenCommand.cs  # Send to kitchen command
    ├── DeliverCommand.cs        # Deliver to customer command
    ├── CancelCommand.cs         # Cancel order command
    └── OrderManager.cs          # Command invoker
```

## Key Concepts

### Table Assignment Strategy

Tables are stored in a `SortedDictionary<int, HashSet<Table>>` keyed by capacity. When finding an available table:
1. Iterate from smallest capacity to largest
2. Find the first table with capacity >= party size that is available at the requested time
3. This ensures optimal table utilization (smallest table that fits)

```csharp
public Table? FindAvailableTable(int partySize, DateTime reservationTime)
{
    foreach (var (capacity, tables) in _tablesByCapacity)
    {
        if (capacity >= partySize)
        {
            foreach (var table in tables)
            {
                if (table.IsAvailableAt(reservationTime))
                    return table;
            }
        }
    }
    return null;
}
```

### Order Item Lifecycle

1. **Created**: Order item starts in `Pending` status
2. **SendToKitchen**: Transitions to `SentToKitchen` when order is placed
3. **Deliver**: Transitions to `Delivered` when food is served
4. **Cancel**: Can cancel from `Pending` or `SentToKitchen` states

## Usage Example

```csharp
// Set up menu
var menu = new Menu();
menu.AddItem(new MenuItem("Burger", "Classic beef burger", 10.99m, Category.Main));
menu.AddItem(new MenuItem("Salad", "Fresh garden salad", 7.99m, Category.Appetizer));
menu.AddItem(new MenuItem("Cake", "Chocolate cake", 5.99m, Category.Dessert));

// Set up layout (tables with capacities 2, 4, 4, 6, 8)
var layout = new Layout([2, 4, 4, 6, 8]);

// Create restaurant
var restaurant = new RestaurantSystem("My Restaurant", menu, layout);

// Make a reservation
var reservation = restaurant.CreateScheduledReservation("Smith", 4, 
    new DateTime(2025, 6, 15, 18, 0, 0));

// Place orders
var burger = menu.GetItem("Burger")!;
var salad = menu.GetItem("Salad")!;
restaurant.OrderItem(reservation.AssignedTable, burger);
restaurant.OrderItem(reservation.AssignedTable, salad);

// Calculate and pay bill
var bill = restaurant.CalculateTableBill(reservation.AssignedTable);
Console.WriteLine($"Total: ${bill}"); // Output: Total: $18.98
```

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `BigDecimal` | `decimal` |
| `HashMap<String, MenuItem>` | `Dictionary<string, MenuItem>` |
| `TreeMap<Integer, Set<Table>>` | `SortedDictionary<int, HashSet<Table>>` |
| `LocalDateTime` | `DateTime` |
| Package `com.restaurant` | Namespace `OodInterview.Restaurant` |
| `getOrDefault()` | `GetValueOrDefault()` |
| `truncatedTo(ChronoUnit.HOURS)` | `new DateTime(year, month, day, hour, 0, 0)` |

## Testing

Run tests with:
```bash
dotnet test tests/OodInterview.Restaurant.Tests
```

The test suite covers:
- Reservation creation and removal (3 tests)
- Order item status transitions (4 tests)
- Table bill calculation (2 tests)
- Command pattern execution (2 tests)
- Layout table finding (2 tests)
- Menu operations (2 tests)
