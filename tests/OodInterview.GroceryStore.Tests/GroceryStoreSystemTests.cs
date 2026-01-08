using OodInterview.GroceryStore.Discount;
using OodInterview.GroceryStore.Discount.Criteria;
using OodInterview.GroceryStore.Discount.Strategy;

namespace OodInterview.GroceryStore.Tests;

public class GroceryStoreSystemTests
{
    [Fact]
    public void TestEndToEnd()
    {
        // Initialize grocery store
        var groceryStoreSystem = new GroceryStoreSystem();

        // Set up catalog, inventory, and example discount
        groceryStoreSystem.AddOrUpdateItem(new Item("Apple", "123", "Fruit", 0.5m));
        groceryStoreSystem.AddOrUpdateItem(new Item("Banana", "124", "Fruit", 1.0m));
        groceryStoreSystem.AddOrUpdateItem(new Item("Gum", "125", "Candy", 4.0m));

        groceryStoreSystem.UpdateInventory("123", 100);
        groceryStoreSystem.UpdateInventory("124", 100);
        groceryStoreSystem.UpdateInventory("125", 100);

        groceryStoreSystem.AddDiscountCampaign(
            new DiscountCampaign(
                "1",
                "Fruit Half Price",
                new CategoryBasedCriteria("Fruit"),
                new PercentageBasedStrategy(50)));

        // Start a new order
        var checkoutSession = groceryStoreSystem.Checkout;

        checkoutSession.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("123")!, 20);
        checkoutSession.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("124")!, 10);
        checkoutSession.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("125")!, 5);

        // Verify total:
        // - 20 Apples @ $0.50 = $10.00, with 50% discount = $5.00
        // - 10 Bananas @ $1.00 = $10.00, with 50% discount = $5.00
        // - 5 Gum @ $4.00 = $20.00, no discount = $20.00
        // Total = $30.00
        var total = checkoutSession.GetOrderTotal();
        Assert.Equal(30.0m, total);

        // Verify change
        var change = checkoutSession.ProcessPayment(100m);
        Assert.Equal(70.0m, change);

        // Verify receipt
        var receipt = checkoutSession.GetReceipt();
        Assert.NotNull(receipt);
        Assert.Contains("Apple", receipt.PrintReceipt());
        Assert.Contains("Banana", receipt.PrintReceipt());
        Assert.Contains("Gum", receipt.PrintReceipt());
    }

    [Fact]
    public void TestCatalogueManagement()
    {
        var groceryStoreSystem = new GroceryStoreSystem();

        // Add items
        groceryStoreSystem.AddOrUpdateItem(new Item("Apple", "123", "Fruit", 0.5m));
        groceryStoreSystem.AddOrUpdateItem(new Item("Banana", "124", "Fruit", 1.0m));
        groceryStoreSystem.AddOrUpdateItem(new Item("Gum", "125", "Candy", 4.0m));

        // Verify items can be retrieved
        Assert.Equal("Apple", groceryStoreSystem.GetItemByBarcode("123")!.Name);
        Assert.Equal("Banana", groceryStoreSystem.GetItemByBarcode("124")!.Name);
        Assert.Equal("Gum", groceryStoreSystem.GetItemByBarcode("125")!.Name);

        // Test update - change item price and category
        groceryStoreSystem.AddOrUpdateItem(new Item("Bread", "123", "Pantry", 0.6m));
        Assert.Equal(0.6m, groceryStoreSystem.GetItemByBarcode("123")!.Price);
        Assert.Equal("Pantry", groceryStoreSystem.GetItemByBarcode("123")!.Category);

        // Test remove item
        groceryStoreSystem.RemoveItem("123");
        Assert.Null(groceryStoreSystem.GetItemByBarcode("123"));
    }

    [Fact]
    public void TestPercentageBasedDiscount()
    {
        var groceryStoreSystem = new GroceryStoreSystem();
        groceryStoreSystem.AddOrUpdateItem(new Item("Apple", "123", "Fruit", 10.0m));
        groceryStoreSystem.AddDiscountCampaign(
            new DiscountCampaign(
                "1",
                "25% Off Fruit",
                new CategoryBasedCriteria("Fruit"),
                new PercentageBasedStrategy(25)));

        var checkout = groceryStoreSystem.Checkout;
        checkout.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("123")!, 1);

        // 10 * (1 - 0.25) = 7.50
        Assert.Equal(7.5m, checkout.GetOrderTotal());
    }

    [Fact]
    public void TestAmountBasedDiscount()
    {
        var groceryStoreSystem = new GroceryStoreSystem();
        groceryStoreSystem.AddOrUpdateItem(new Item("Apple", "123", "Fruit", 10.0m));
        groceryStoreSystem.AddDiscountCampaign(
            new DiscountCampaign(
                "1",
                "$3 Off Apples",
                new ItemBasedCriteria("123"),
                new AmountBasedStrategy(3.0m)));

        var checkout = groceryStoreSystem.Checkout;
        checkout.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("123")!, 1);

        // 10 - 3 = 7.00
        Assert.Equal(7.0m, checkout.GetOrderTotal());
    }

    [Fact]
    public void TestNoDiscountApplied()
    {
        var groceryStoreSystem = new GroceryStoreSystem();
        groceryStoreSystem.AddOrUpdateItem(new Item("Candy", "123", "Candy", 5.0m));
        groceryStoreSystem.AddDiscountCampaign(
            new DiscountCampaign(
                "1",
                "Fruit Half Price",
                new CategoryBasedCriteria("Fruit"),
                new PercentageBasedStrategy(50)));

        var checkout = groceryStoreSystem.Checkout;
        checkout.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("123")!, 2);

        // Candy doesn't qualify for Fruit discount
        // 5 * 2 = 10.00
        Assert.Equal(10.0m, checkout.GetOrderTotal());
    }

    [Fact]
    public void TestMultipleItemsWithMixedDiscounts()
    {
        var groceryStoreSystem = new GroceryStoreSystem();
        groceryStoreSystem.AddOrUpdateItem(new Item("Apple", "123", "Fruit", 2.0m));
        groceryStoreSystem.AddOrUpdateItem(new Item("Bread", "124", "Bakery", 3.0m));
        groceryStoreSystem.AddOrUpdateItem(new Item("Milk", "125", "Dairy", 4.0m));

        groceryStoreSystem.AddDiscountCampaign(
            new DiscountCampaign(
                "1",
                "Fruit 50% Off",
                new CategoryBasedCriteria("Fruit"),
                new PercentageBasedStrategy(50)));

        groceryStoreSystem.AddDiscountCampaign(
            new DiscountCampaign(
                "2",
                "Dairy 25% Off",
                new CategoryBasedCriteria("Dairy"),
                new PercentageBasedStrategy(25)));

        var checkout = groceryStoreSystem.Checkout;
        checkout.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("123")!, 2); // 2 * 2 = 4, with 50% = 2
        checkout.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("124")!, 1); // 3 * 1 = 3, no discount = 3
        checkout.AddItemToOrder(groceryStoreSystem.GetItemByBarcode("125")!, 1); // 4 * 1 = 4, with 25% = 3

        // Total = 2 + 3 + 3 = 8
        Assert.Equal(8.0m, checkout.GetOrderTotal());
    }

    [Fact]
    public void TestInventoryManagement()
    {
        var groceryStoreSystem = new GroceryStoreSystem();
        groceryStoreSystem.AddOrUpdateItem(new Item("Apple", "123", "Fruit", 1.0m));

        // Initially no stock
        var inventory = new Inventory();
        Assert.Equal(0, inventory.GetStock("123"));

        // Add stock
        inventory.AddStock("123", 50);
        Assert.Equal(50, inventory.GetStock("123"));

        // Reduce stock
        inventory.ReduceStock("123", 20);
        Assert.Equal(30, inventory.GetStock("123"));
    }
}
