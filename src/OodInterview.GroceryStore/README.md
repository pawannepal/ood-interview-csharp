# Grocery Store System

A grocery store system that manages inventory, processes orders, and applies discounts.

## Project Structure

```
OodInterview.GroceryStore/
├── GroceryStoreSystem.cs      # Main system class
├── Catalog.cs                 # Product catalog management
├── Inventory.cs               # Stock level tracking
├── Checkout.cs                # Checkout process handler
├── Order.cs                   # Customer order
├── OrderItem.cs               # Individual order line item
├── Item.cs                    # Product item
├── Receipt.cs                 # Order receipt
└── Discount/
    ├── DiscountCampaign.cs    # Discount campaign manager
    ├── Criteria/
    │   ├── IDiscountCriteria.cs       # Criteria interface
    │   ├── CategoryBasedCriteria.cs   # Category-based filtering
    │   └── ItemBasedCriteria.cs       # Item-specific filtering
    └── Strategy/
        ├── IDiscountCalculationStrategy.cs  # Strategy interface
        ├── PercentageBasedStrategy.cs       # Percentage discount
        └── AmountBasedStrategy.cs           # Fixed amount discount
```

## Design Patterns Used

### 1. Strategy Pattern

The **Strategy Pattern** is used for discount calculation, allowing different discount algorithms to be applied interchangeably.

```
    IDiscountCalculationStrategy
              │
    ┌─────────┴─────────┐
    │                   │
PercentageBased    AmountBased
  Strategy          Strategy
```

**Key benefits:**
- Easy to add new discount types (e.g., buy-one-get-one)
- Runtime selection of discount algorithms
- Clean separation of calculation logic

**Example:**
```csharp
// Percentage-based discount (50% off)
var percentageStrategy = new PercentageBasedStrategy(50);

// Fixed amount discount ($3 off)
var amountStrategy = new AmountBasedStrategy(3.0m);

// Both can be used interchangeably in DiscountCampaign
var campaign = new DiscountCampaign("1", "Sale", criteria, percentageStrategy);
```

### 2. Strategy Pattern (Criteria)

A second application of the **Strategy Pattern** for determining discount eligibility.

```
    IDiscountCriteria
          │
    ┌─────┴─────┐
    │           │
CategoryBased  ItemBased
  Criteria     Criteria
```

**Criteria Types:**
- `CategoryBasedCriteria` - Applies discount to all items in a category (e.g., "Fruit")
- `ItemBasedCriteria` - Applies discount to specific items by barcode

**Example:**
```csharp
// Apply to all Fruit items
var fruitCriteria = new CategoryBasedCriteria("Fruit");

// Apply to specific item
var appleCriteria = new ItemBasedCriteria("123");
```

### 3. Facade Pattern

The `GroceryStoreSystem` acts as a **Facade**, providing a simplified interface to the complex subsystem of Catalog, Inventory, Checkout, and Discounts.

```
     GroceryStoreSystem (Facade)
              │
    ┌─────────┼─────────┬──────────┐
    │         │         │          │
 Catalog  Inventory  Checkout  Discounts
```

## Usage Example

```csharp
using OodInterview.GroceryStore;
using OodInterview.GroceryStore.Discount;
using OodInterview.GroceryStore.Discount.Criteria;
using OodInterview.GroceryStore.Discount.Strategy;

// Initialize store
var store = new GroceryStoreSystem();

// Add products to catalog
store.AddOrUpdateItem(new Item("Apple", "123", "Fruit", 0.50m));
store.AddOrUpdateItem(new Item("Banana", "124", "Fruit", 1.00m));
store.AddOrUpdateItem(new Item("Gum", "125", "Candy", 4.00m));

// Set up inventory
store.UpdateInventory("123", 100);
store.UpdateInventory("124", 100);
store.UpdateInventory("125", 100);

// Create a discount campaign: 50% off all Fruit
store.AddDiscountCampaign(new DiscountCampaign(
    "1",
    "Fruit Half Price",
    new CategoryBasedCriteria("Fruit"),
    new PercentageBasedStrategy(50)));

// Process an order
var checkout = store.Checkout;
checkout.AddItemToOrder(store.GetItemByBarcode("123")!, 20);  // 20 Apples
checkout.AddItemToOrder(store.GetItemByBarcode("124")!, 10);  // 10 Bananas
checkout.AddItemToOrder(store.GetItemByBarcode("125")!, 5);   // 5 Gum

// Calculate total (Fruit items have 50% discount)
var total = checkout.GetOrderTotal();  // $30.00

// Process payment and get change
var change = checkout.ProcessPayment(100m);  // $70.00

// Generate receipt
var receipt = checkout.GetReceipt();
Console.WriteLine(receipt.PrintReceipt());
```

## Order Calculation

### Without Discounts
```
Item Price × Quantity = Line Total
$0.50 × 20 = $10.00 (Apples)
$1.00 × 10 = $10.00 (Bananas)  
$4.00 × 5  = $20.00 (Gum)
                      ───────
Subtotal:             $40.00
```

### With 50% Fruit Discount
```
Line Total × (1 - Discount%) = Discounted Total
$10.00 × 0.5 = $5.00  (Apples - 50% off)
$10.00 × 0.5 = $5.00  (Bananas - 50% off)
$20.00 × 1.0 = $20.00 (Gum - no discount)
                        ───────
Total:                  $30.00
```

## Key Features

- **Catalog Management**: Add, update, and remove products
- **Inventory Tracking**: Track stock levels for each product
- **Flexible Discounts**: Apply category-based or item-specific discounts
- **Multiple Discount Strategies**: Percentage or fixed amount discounts
- **Best Discount Selection**: When multiple discounts apply, the best one is chosen
- **Receipt Generation**: Generate formatted receipts with itemized details

## Java to C# Conversion Notes

| Java | C# |
|------|-----|
| `BigDecimal` | `decimal` |
| `HashMap<K, V>` | `Dictionary<K, V>` |
| `ArrayList<T>` | `List<T>` |
| `Collections.unmodifiableList()` | `list.AsReadOnly()` |
| `UUID.randomUUID()` | `Guid.NewGuid()` |
| `map.getOrDefault(key, default)` | `dict.GetValueOrDefault(key, default)` |
| `stream().map().reduce()` | LINQ `.Sum()` |
| `StringBuilder` | `System.Text.StringBuilder` |
| `Date` | `DateTime` |

## Running Tests

```bash
# From repository root
dotnet test tests/OodInterview.GroceryStore.Tests
```
