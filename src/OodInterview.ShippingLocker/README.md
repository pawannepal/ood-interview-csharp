# OodInterview.ShippingLocker

An Amazon Locker-style system for managing package delivery to automated lockers, with support for account-based pricing, access code generation, and storage period management.

## Overview

This project simulates a shipping locker system with capabilities for:
- **Package Management**: Creating and tracking packages with dimensions
- **Locker Assignment**: Automatically assigning packages to appropriately-sized lockers
- **Access Code Generation**: Secure 6-digit codes for package pickup
- **Storage Charges**: Time-based billing with free period and maximum storage enforcement
- **Notifications**: Observer pattern for notifying users of package events

## Design Patterns Used

### 1. Strategy Pattern (Pricing)

The locker size enum with extension methods implements a variation of the Strategy Pattern for pricing:

```csharp
public enum LockerSize
{
    Small,   // $5/day,  10x10x10
    Medium,  // $10/day, 20x20x20
    Large    // $15/day, 30x30x30
}

public static class LockerSizeExtensions
{
    public static decimal GetDailyCharge(this LockerSize size) => size switch
    {
        LockerSize.Small => 5.00m,
        LockerSize.Medium => 10.00m,
        LockerSize.Large => 15.00m,
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };
}
```

### 2. Observer Pattern (Notifications)

The notification system uses the Observer Pattern to decouple locker events from notification delivery:

```csharp
public interface INotificationService
{
    void SendNotification(string message, Account user);
}

public class EmailNotificationService : INotificationService
{
    public void SendNotification(string message, Account user)
    {
        // Send email notification
    }
}

// Usage in LockerManager
_notificationService.SendNotification($"Package assigned to locker {code}", pkg.User);
```

### 3. Factory Pattern (Package Sizing)

The `BasicShippingPackage` class automatically determines the appropriate locker size:

```csharp
public LockerSize GetLockerSize()
{
    foreach (var size in Enum.GetValues<LockerSize>())
    {
        if (size.GetWidth() >= Width &&
            size.GetHeight() >= Height &&
            size.GetDepth() >= Depth)
        {
            return size;
        }
    }
    throw new PackageIncompatibleException("No locker size available");
}
```

### 4. Facade Pattern

The `ShippingLockerSystem` and `LockerManager` classes provide simplified interfaces to the complex subsystem:

```csharp
public class ShippingLockerSystem
{
    public Locker AssignPackage(IShippingPackage package, DateTime deliveryDate)
    {
        return _lockerManager.AssignPackage(package, deliveryDate);
    }

    public Locker? PickUpPackage(string accessCode)
    {
        return _lockerManager.PickUpPackage(accessCode);
    }
}
```

## Architecture

```
OodInterview.ShippingLocker/
├── ShippingLockerSystem.cs          # Main facade
├── INotificationService.cs          # Notification interface
├── EmailNotificationService.cs      # Email notification implementation
├── Account/
│   ├── Account.cs                   # User account with usage charges
│   └── AccountLockerPolicy.cs       # Defines free/max storage periods
├── Locker/
│   ├── LockerSize.cs                # Locker size enum with extensions
│   ├── Locker.cs                    # Individual locker entity
│   ├── Site.cs                      # Collection of lockers at a location
│   └── LockerManager.cs             # Manages locker operations
└── Package/
    ├── ShippingStatus.cs            # Package status enum
    ├── IShippingPackage.cs          # Package interface
    ├── BasicShippingPackage.cs      # Package implementation
    ├── PackageIncompatibleException.cs
    ├── NoLockerAvailableException.cs
    └── MaximumStoragePeriodExceededException.cs
```

## Key Concepts

### Locker Sizes and Pricing

| Size | Dimensions | Daily Charge |
|------|------------|--------------|
| Small | 10×10×10 | $5.00 |
| Medium | 20×20×20 | $10.00 |
| Large | 30×30×30 | $15.00 |

### Package Lifecycle

```
Created → Pending → InLocker → Retrieved
                          ↓
                      Expired (if max storage exceeded)
```

### Account Locker Policy

Each account has a policy that defines:
- **Free Period Days**: Number of days of free storage
- **Maximum Period Days**: Maximum allowed storage duration

```csharp
var policy = new AccountLockerPolicy(
    freePeriodDays: 3,      // First 3 days free
    maximumPeriodDays: 14   // Max 14 days storage
);
```

### Storage Charge Calculation

```csharp
public decimal CalculateStorageCharges()
{
    var totalDaysUsed = (DateTime.Now - assignmentDate).TotalDays;
    
    // Check if exceeds maximum period
    if (totalDaysUsed > policy.MaximumPeriodDays)
    {
        throw new MaximumStoragePeriodExceededException(...);
    }
    
    // Calculate chargeable days (excluding free period)
    var chargeableDays = Math.Max(0, totalDaysUsed - policy.FreePeriodDays);
    return lockerSize.GetDailyCharge() * chargeableDays;
}
```

## Usage Example

```csharp
// Set up locker site
var lockerConfig = new Dictionary<LockerSize, int>
{
    { LockerSize.Small, 10 },
    { LockerSize.Medium, 10 },
    { LockerSize.Large, 10 }
};

// Create account with policy
var policy = new AccountLockerPolicy(freePeriodDays: 0, maximumPeriodDays: 10);
var account = new Account("user123", "John Doe", policy);

var accounts = new Dictionary<string, Account> { { "user123", account } };

// Create shipping locker system
var system = ShippingLockerSystem.Create(
    lockerConfig, 
    accounts, 
    new EmailNotificationService()
);

// Create and assign package
var package = new BasicShippingPackage(
    orderId: "ORD-001",
    user: account,
    width: 10.00m,
    height: 10.00m,
    depth: 10.00m
);

var locker = system.AssignPackage(package, DateTime.Now);
var accessCode = locker.AccessCode; // e.g., "847293"

// Customer picks up package
var retrievedLocker = system.PickUpPackage(accessCode);
Console.WriteLine($"Charges: ${account.UsageCharges}");
```

## Exception Handling

The system uses custom exceptions for specific error conditions:

| Exception | When Thrown |
|-----------|-------------|
| `PackageIncompatibleException` | Package dimensions exceed largest locker |
| `NoLockerAvailableException` | No available locker of required size |
| `MaximumStoragePeriodExceededException` | Storage duration exceeds account policy |

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `BigDecimal` | `decimal` |
| `HashMap<K, V>` | `Dictionary<K, V>` |
| `HashSet<T>` | `HashSet<T>` |
| `Date` | `DateTime` |
| `enum with fields` | `enum` + extension methods |
| Interface prefix: none | Interface prefix: `I` |
| `getSize()` | `Size` property |
| `new Random().nextInt()` | `Random.Shared.Next()` |

## Testing

Run tests with:
```bash
dotnet test tests/OodInterview.ShippingLocker.Tests
```

The test suite covers:
- Package assignment to lockers (3 tests)
- Package pickup and status transitions (2 tests)
- Storage charge calculations with free periods (2 tests)
- Account operations (3 tests)
- Locker lifecycle (4 tests)
- Package sizing and exceptions (4 tests)
- Site operations (3 tests)
- Locker size dimensions and charges (6 tests)
