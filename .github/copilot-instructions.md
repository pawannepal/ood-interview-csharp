# Copilot Instructions - OOD Interview .NET Core 10 Port

This document serves as the **source of truth** for porting the ByteByteGoHq/ood-interview Java repository to .NET Core 10.

## Source Repository
- **Original Repository**: https://github.com/ByteByteGoHq/ood-interview
- **Original Language**: Java 17+
- **Target Framework**: .NET 10.0

## Project Overview

This is a companion code repository for "Object-Oriented Design Interview" by ByteByteGo. It contains 11 OOD examples demonstrating common interview scenarios.

## Projects to Port

| Java Project | .NET Project Name | Description |
|-------------|------------------|-------------|
| `atm` | `OodInterview.Atm` | ATM System with state pattern |
| `blackjack` | `OodInterview.Blackjack` | Blackjack card game |
| `elevator_system` | `OodInterview.Elevator` | Elevator system with observer pattern |
| `file_search` | `OodInterview.FileSearch` | Unix-style file search with predicates |
| `grocery_store` | `OodInterview.GroceryStore` | Grocery store with discounts |
| `movie_ticket` | `OodInterview.MovieTicket` | Movie ticket booking system |
| `parking_lot` | `OodInterview.ParkingLot` | Parking lot management |
| `restaurant` | `OodInterview.Restaurant` | Restaurant management with command pattern |
| `shipping_locker` | `OodInterview.ShippingLocker` | Amazon locker system |
| `tic_tac_toe` | `OodInterview.TicTacToe` | Tic-Tac-Toe game |
| `vending_machine` | `OodInterview.VendingMachine` | Vending machine with state pattern |

## Solution Structure

```
OodInterview.sln
├── src/
│   ├── OodInterview.Atm/
│   ├── OodInterview.Blackjack/
│   ├── OodInterview.Elevator/
│   ├── OodInterview.FileSearch/
│   ├── OodInterview.GroceryStore/
│   ├── OodInterview.MovieTicket/
│   ├── OodInterview.ParkingLot/
│   ├── OodInterview.Restaurant/
│   ├── OodInterview.ShippingLocker/
│   ├── OodInterview.TicTacToe/
│   └── OodInterview.VendingMachine/
└── tests/
    ├── OodInterview.Atm.Tests/
    ├── OodInterview.Blackjack.Tests/
    ├── OodInterview.Elevator.Tests/
    ├── OodInterview.FileSearch.Tests/
    ├── OodInterview.GroceryStore.Tests/
    ├── OodInterview.MovieTicket.Tests/
    ├── OodInterview.ParkingLot.Tests/
    ├── OodInterview.Restaurant.Tests/
    ├── OodInterview.ShippingLocker.Tests/
    ├── OodInterview.TicTacToe.Tests/
    └── OodInterview.VendingMachine.Tests/
```

## Java to C# Conversion Rules

### Naming Conventions
| Java | C# |
|------|-----|
| `camelCase` methods | `PascalCase` methods |
| `camelCase` fields | `_camelCase` private fields |
| `SCREAMING_CASE` constants | `PascalCase` constants |
| Package names (`com.example`) | Namespaces (`Com.Example`) |
| Interface prefix: none | Interface prefix: `I` (e.g., `IVehicle`) |

### Type Mappings
| Java | C# |
|------|-----|
| `String` | `string` |
| `int`, `Integer` | `int` |
| `long`, `Long` | `long` |
| `double`, `Double` | `double` |
| `boolean`, `Boolean` | `bool` |
| `BigDecimal` | `decimal` |
| `List<T>` | `List<T>` or `IList<T>` |
| `Map<K, V>` | `Dictionary<K, V>` or `IDictionary<K, V>` |
| `Set<T>` | `HashSet<T>` or `ISet<T>` |
| `Optional<T>` | `T?` (nullable) |
| `LocalDateTime` | `DateTime` |
| `Duration` | `TimeSpan` |

### Pattern Mappings
| Java Pattern | C# Pattern |
|-------------|------------|
| `record` | `record` (C# 9+) or class with init properties |
| `sealed interface` | `interface` (C# doesn't enforce sealed on interfaces) |
| `enum with fields` | `enum` with extension methods or sealed classes |
| `@Override` | `override` keyword |
| `@FunctionalInterface` | `Func<>` or `Action<>` delegates |
| `Stream API` | LINQ |
| Getters (`getX()`) | Properties (`X { get; }`) |
| Setters (`setX(val)`) | Properties (`X { set; }`) |

### Collection Operations
| Java | C# |
|------|-----|
| `list.stream().filter().map().collect()` | `list.Where().Select().ToList()` |
| `list.forEach()` | `list.ForEach()` or `foreach` loop |
| `list.isEmpty()` | `list.Count == 0` or `!list.Any()` |
| `map.getOrDefault(key, default)` | `map.GetValueOrDefault(key, default)` |
| `list.add(item)` | `list.Add(item)` |
| `list.remove(item)` | `list.Remove(item)` |
| `Collections.unmodifiableList()` | `list.AsReadOnly()` or `IReadOnlyList<T>` |

### Exception Handling
| Java | C# |
|------|-----|
| `throws Exception` | No checked exceptions in C# |
| `RuntimeException` | `Exception` |
| Custom exception extending `RuntimeException` | Custom exception extending `Exception` |

### Access Modifiers
| Java | C# |
|------|-----|
| `public` | `public` |
| `private` | `private` |
| `protected` | `protected` |
| (package-private, no modifier) | `internal` |

### Common Patterns

#### Java Enum with Fields
```java
public enum LockerSize {
    SMALL("Small", new BigDecimal("5.00")),
    MEDIUM("Medium", new BigDecimal("10.00"));
    
    private final String name;
    private final BigDecimal price;
    
    LockerSize(String name, BigDecimal price) {
        this.name = name;
        this.price = price;
    }
}
```

#### C# Equivalent
```csharp
public enum LockerSize
{
    Small,
    Medium
}

public static class LockerSizeExtensions
{
    public static string GetDisplayName(this LockerSize size) => size switch
    {
        LockerSize.Small => "Small",
        LockerSize.Medium => "Medium",
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public static decimal GetPrice(this LockerSize size) => size switch
    {
        LockerSize.Small => 5.00m,
        LockerSize.Medium => 10.00m,
        _ => throw new ArgumentOutOfRangeException()
    };
}
```

#### Java Interface
```java
public interface Vehicle {
    String getLicensePlate();
    VehicleSize getSize();
}
```

#### C# Equivalent
```csharp
public interface IVehicle
{
    string LicensePlate { get; }
    VehicleSize Size { get; }
}
```

#### Java State Pattern
```java
public interface VendingMachineState {
    void insertMoney(VendingMachine vm, double amount);
    void selectProduct(VendingMachine vm, String code);
}
```

#### C# Equivalent
```csharp
public interface IVendingMachineState
{
    void InsertMoney(VendingMachine vm, decimal amount);
    void SelectProduct(VendingMachine vm, string code);
}
```

## Testing Framework
- **Java**: JUnit 5
- **C#**: xUnit.net

### Test Attribute Mappings
| JUnit 5 | xUnit |
|---------|-------|
| `@Test` | `[Fact]` |
| `@ParameterizedTest` | `[Theory]` |
| `@BeforeEach` | Constructor or `IClassFixture<T>` |
| `@AfterEach` | `IDisposable.Dispose()` |
| `@BeforeAll` | `IClassFixture<T>` |
| `@DisplayName("...")` | `[Fact(DisplayName = "...")]` |
| `assertEquals(expected, actual)` | `Assert.Equal(expected, actual)` |
| `assertTrue(condition)` | `Assert.True(condition)` |
| `assertThrows(Ex.class, () -> ...)` | `Assert.Throws<Ex>(() => ...)` |

## Design Patterns Used in Original Repo

1. **State Pattern**: ATM, Vending Machine
2. **Strategy Pattern**: Elevator dispatching, Fare calculation, Pricing
3. **Observer Pattern**: Elevator system
4. **Command Pattern**: Restaurant orders
5. **Factory Pattern**: Various object creation
6. **Composite Pattern**: File search predicates (AND/OR)

## Implementation Guidelines

1. **Use Records for DTOs**: Use C# records for immutable data transfer objects
2. **Use Primary Constructors**: Use C# 12 primary constructors where appropriate
3. **Use File-Scoped Namespaces**: Use file-scoped namespaces for cleaner code
4. **Use Target-Typed New**: Use `new()` where type is obvious
5. **Use Pattern Matching**: Leverage C# pattern matching for cleaner conditionals
6. **Use Nullable Reference Types**: Enable nullable reference types
7. **Use Collection Expressions**: Use C# 12 collection expressions where appropriate

## Build Commands

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Run all tests
dotnet test

# Run specific project tests
dotnet test tests/OodInterview.ParkingLot.Tests

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

## Project References

Each test project should reference its corresponding source project:
- `OodInterview.Atm.Tests` → `OodInterview.Atm`
- etc.

## NuGet Packages Required

### Source Projects
- None required (standard library only)

### Test Projects
- `xunit` (latest)
- `xunit.runner.visualstudio` (latest)
- `Microsoft.NET.Test.Sdk` (latest)
- `coverlet.collector` (latest) - optional for coverage

## Progress Tracking

Use this section to track porting progress:

- [ ] Solution structure created
- [ ] OodInterview.Atm ported
- [ ] OodInterview.Blackjack ported
- [ ] OodInterview.Elevator ported
- [ ] OodInterview.FileSearch ported
- [ ] OodInterview.GroceryStore ported
- [ ] OodInterview.MovieTicket ported
- [ ] OodInterview.ParkingLot ported
- [ ] OodInterview.Restaurant ported
- [ ] OodInterview.ShippingLocker ported
- [ ] OodInterview.TicTacToe ported
- [ ] OodInterview.VendingMachine ported
- [ ] All tests passing

## Notes

- Preserve the original design patterns and architecture
- Maintain the same test coverage as the original Java code
- Use idiomatic C# - don't just transliterate Java to C#
- Add XML documentation comments for public APIs
- Follow .NET naming conventions strictly
