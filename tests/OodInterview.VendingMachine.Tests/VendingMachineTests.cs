using OodInterview.VendingMachine;

namespace OodInterview.VendingMachine.Tests;

public class VendingMachineTests
{
    [Fact]
    public void TestEndToEndVending()
    {
        // Arrange - Initialize vending machine
        var machine = new VendingMachine();

        // Set up products and inventory
        var itemA = new Product("a", "Product A", 1.00m);
        var itemB = new Product("b", "Product B", 1.50m);
        var itemC = new Product("c", "Product C", 1.25m);

        machine.SetRack(new Dictionary<string, Rack>
        {
            { "A1", new Rack("A1", itemA, 10) },
            { "A2", new Rack("A2", itemB, 5) },
            { "A3", new Rack("A3", itemC, 15) }
        });

        // First Purchase Transaction
        machine.InsertMoney(20.00m);
        machine.ChooseProduct("A2");
        var transaction = machine.ConfirmTransaction();

        Assert.Equal(itemB, transaction.Product);
        Assert.Equal(18.50m, transaction.TotalAmount);

        // Second Purchase Transaction
        machine.InsertMoney(6.00m);
        machine.ChooseProduct("A2");
        transaction = machine.ConfirmTransaction();

        Assert.Equal(itemB, transaction.Product);
        Assert.Equal(4.50m, transaction.TotalAmount);

        // Verify transaction history
        var history = machine.GetTransactionHistory();
        Assert.Equal(2, history.Count);
        Assert.Equal(itemB, history[0].Product);
        Assert.Equal(itemB, history[1].Product);

        // Verify inventory levels
        var inventory = machine.GetInventoryManager();
        Assert.Equal(3, inventory.GetRack("A2").ProductCount);
    }

    [Fact]
    public void TestInsufficientFunds_ShouldThrow()
    {
        // Arrange
        var machine = new VendingMachine();
        var itemB = new Product("b", "Product B", 1.50m);

        machine.SetRack(new Dictionary<string, Rack>
        {
            { "A2", new Rack("A2", itemB, 5) }
        });

        // Act - Insert insufficient funds
        machine.InsertMoney(1.00m);
        machine.ChooseProduct("A2");

        // Assert
        var exception = Assert.Throws<InvalidTransactionException>(() => machine.ConfirmTransaction());
        Assert.Equal("Insufficient fund", exception.Message);
    }

    [Fact]
    public void TestCancelTransaction_ReturnsMoney()
    {
        // Arrange
        var machine = new VendingMachine();
        var itemA = new Product("a", "Product A", 1.00m);

        machine.SetRack(new Dictionary<string, Rack>
        {
            { "A1", new Rack("A1", itemA, 10) }
        });

        // Act - Insert money and cancel
        machine.InsertMoney(5.00m);
        machine.ChooseProduct("A1");
        machine.CancelTransaction();

        // Try to buy again without inserting money
        machine.ChooseProduct("A1");

        // Assert - should fail due to no funds
        var exception = Assert.Throws<InvalidTransactionException>(() => machine.ConfirmTransaction());
        Assert.Equal("Insufficient fund", exception.Message);
    }

    [Fact]
    public void TestNoProductSelected_ShouldThrow()
    {
        // Arrange
        var machine = new VendingMachine();
        var itemA = new Product("a", "Product A", 1.00m);

        machine.SetRack(new Dictionary<string, Rack>
        {
            { "A1", new Rack("A1", itemA, 10) }
        });

        // Act - Insert money but don't select product
        machine.InsertMoney(5.00m);

        // Assert
        var exception = Assert.Throws<InvalidTransactionException>(() => machine.ConfirmTransaction());
        Assert.Equal("Invalid product selection", exception.Message);
    }

    [Fact]
    public void TestOutOfStock_ShouldThrow()
    {
        // Arrange
        var machine = new VendingMachine();
        var itemA = new Product("a", "Product A", 1.00m);

        machine.SetRack(new Dictionary<string, Rack>
        {
            { "A1", new Rack("A1", itemA, 0) } // Empty rack
        });

        // Act
        machine.InsertMoney(5.00m);
        machine.ChooseProduct("A1");

        // Assert
        var exception = Assert.Throws<InvalidTransactionException>(() => machine.ConfirmTransaction());
        Assert.Equal("Insufficient inventory for product.", exception.Message);
    }

    [Fact]
    public void TestMultiplePurchasesDifferentProducts()
    {
        // Arrange
        var machine = new VendingMachine();
        var itemA = new Product("a", "Product A", 1.00m);
        var itemB = new Product("b", "Product B", 1.50m);
        var itemC = new Product("c", "Product C", 1.25m);

        machine.SetRack(new Dictionary<string, Rack>
        {
            { "A1", new Rack("A1", itemA, 10) },
            { "A2", new Rack("A2", itemB, 5) },
            { "A3", new Rack("A3", itemC, 15) }
        });

        // Purchase A
        machine.InsertMoney(2.00m);
        machine.ChooseProduct("A1");
        var txA = machine.ConfirmTransaction();
        Assert.Equal(itemA, txA.Product);
        Assert.Equal(1.00m, txA.TotalAmount); // 2.00 - 1.00 = 1.00 change

        // Purchase B
        machine.InsertMoney(2.00m);
        machine.ChooseProduct("A2");
        var txB = machine.ConfirmTransaction();
        Assert.Equal(itemB, txB.Product);
        Assert.Equal(0.50m, txB.TotalAmount); // 2.00 - 1.50 = 0.50 change

        // Purchase C
        machine.InsertMoney(1.25m);
        machine.ChooseProduct("A3");
        var txC = machine.ConfirmTransaction();
        Assert.Equal(itemC, txC.Product);
        Assert.Equal(0.00m, txC.TotalAmount); // Exact change

        // Verify inventory
        var inventory = machine.GetInventoryManager();
        Assert.Equal(9, inventory.GetRack("A1").ProductCount);
        Assert.Equal(4, inventory.GetRack("A2").ProductCount);
        Assert.Equal(14, inventory.GetRack("A3").ProductCount);

        // Verify history
        Assert.Equal(3, machine.GetTransactionHistory().Count);
    }
}

public class ProductTests
{
    [Fact]
    public void Product_HasCorrectProperties()
    {
        // Arrange & Act
        var product = new Product("abc", "Test Product", 2.50m);

        // Assert
        Assert.Equal("abc", product.ProductCode);
        Assert.Equal("Test Product", product.Description);
        Assert.Equal(2.50m, product.UnitPrice);
    }
}

public class RackTests
{
    [Fact]
    public void Rack_HasCorrectProperties()
    {
        // Arrange
        var product = new Product("a", "Product A", 1.00m);

        // Act
        var rack = new Rack("A1", product, 10);

        // Assert
        Assert.Equal("A1", rack.RackCode);
        Assert.Equal(product, rack.Product);
        Assert.Equal(10, rack.ProductCount);
    }

    [Fact]
    public void SetCount_UpdatesProductCount()
    {
        // Arrange
        var product = new Product("a", "Product A", 1.00m);
        var rack = new Rack("A1", product, 10);

        // Act
        rack.SetCount(5);

        // Assert
        Assert.Equal(5, rack.ProductCount);
    }
}

public class PaymentProcessorTests
{
    [Fact]
    public void AddBalance_IncreasesBalance()
    {
        // Arrange
        var processor = new PaymentProcessor();

        // Act
        processor.AddBalance(5.00m);
        processor.AddBalance(3.00m);

        // Assert
        Assert.Equal(8.00m, processor.CurrentBalance);
    }

    [Fact]
    public void Charge_DecreasesBalance()
    {
        // Arrange
        var processor = new PaymentProcessor();
        processor.AddBalance(10.00m);

        // Act
        processor.Charge(3.50m);

        // Assert
        Assert.Equal(6.50m, processor.CurrentBalance);
    }

    [Fact]
    public void ReturnChange_ReturnsBalanceAndResets()
    {
        // Arrange
        var processor = new PaymentProcessor();
        processor.AddBalance(5.00m);

        // Act
        var change = processor.ReturnChange();

        // Assert
        Assert.Equal(5.00m, change);
        Assert.Equal(0.00m, processor.CurrentBalance);
    }
}

public class InventoryManagerTests
{
    [Fact]
    public void GetProductInRack_ReturnsCorrectProduct()
    {
        // Arrange
        var inventory = new InventoryManager();
        var product = new Product("a", "Product A", 1.00m);
        var rack = new Rack("A1", product, 10);

        inventory.UpdateRack(new Dictionary<string, Rack> { { "A1", rack } });

        // Act
        var result = inventory.GetProductInRack("A1");

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public void DispenseProductFromRack_DecrementsCount()
    {
        // Arrange
        var inventory = new InventoryManager();
        var product = new Product("a", "Product A", 1.00m);
        var rack = new Rack("A1", product, 10);

        inventory.UpdateRack(new Dictionary<string, Rack> { { "A1", rack } });

        // Act
        inventory.DispenseProductFromRack(rack);

        // Assert
        Assert.Equal(9, rack.ProductCount);
    }
}

public class TransactionTests
{
    [Fact]
    public void Transaction_CanSetProperties()
    {
        // Arrange
        var product = new Product("a", "Product A", 1.00m);
        var rack = new Rack("A1", product, 10);

        // Act
        var transaction = new Transaction
        {
            Product = product,
            Rack = rack,
            TotalAmount = 4.00m
        };

        // Assert
        Assert.Equal(product, transaction.Product);
        Assert.Equal(rack, transaction.Rack);
        Assert.Equal(4.00m, transaction.TotalAmount);
    }
}
