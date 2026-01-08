using OodInterview.Restaurant;
using OodInterview.Restaurant.Command;
using OodInterview.Restaurant.Menu;
using OodInterview.Restaurant.Reservation;
using OodInterview.Restaurant.Table;

namespace OodInterview.Restaurant.Tests;

public class RestaurantSystemTests
{
    private readonly Menu.Menu _menu;
    private readonly Layout _layout;
    private readonly RestaurantSystem _restaurant;

    public RestaurantSystemTests()
    {
        _menu = new Menu.Menu();
        _menu.AddItem(new MenuItem("Burger", "Classic beef burger", 10.99m, Category.Main));
        _menu.AddItem(new MenuItem("Salad", "Fresh garden salad", 7.99m, Category.Appetizer));
        _menu.AddItem(new MenuItem("Cake", "Chocolate cake", 5.99m, Category.Dessert));
        
        _layout = new Layout([2, 4, 4, 6, 8]);
        _restaurant = new RestaurantSystem("Test Restaurant", _menu, _layout);
    }

    [Fact]
    public void CreateReservation_ShouldAssignTable()
    {
        // Arrange
        var reservationTime = new DateTime(2025, 6, 15, 18, 0, 0);
        
        // Act
        var reservation = _restaurant.CreateScheduledReservation("Smith", 4, reservationTime);
        
        // Assert
        Assert.NotNull(reservation);
        Assert.Equal("Smith", reservation.PartyName);
        Assert.Equal(4, reservation.PartySize);
        Assert.True(reservation.AssignedTable.Capacity >= 4);
    }

    [Fact]
    public void FindAvailableTimeSlots_ShouldReturnValidSlots()
    {
        // Arrange
        var rangeStart = new DateTime(2025, 6, 15, 17, 0, 0);
        var rangeEnd = new DateTime(2025, 6, 15, 21, 0, 0);
        
        // Act
        var slots = _restaurant.FindAvailableTimeSlots(rangeStart, rangeEnd, 4);
        
        // Assert
        Assert.NotEmpty(slots);
        Assert.All(slots, slot => 
        {
            Assert.True(slot >= rangeStart);
            Assert.True(slot <= rangeEnd);
        });
    }

    [Fact]
    public void RemoveReservation_ShouldFreeUpTable()
    {
        // Arrange
        var reservationTime = new DateTime(2025, 6, 15, 18, 0, 0);
        var reservation = _restaurant.CreateScheduledReservation("Smith", 4, reservationTime);
        
        // Act
        _restaurant.RemoveReservation("Smith", 4, reservationTime);
        
        // Assert
        Assert.DoesNotContain(_restaurant.ReservationManager.Reservations, 
            r => r.PartyName == "Smith" && r.Time == reservationTime);
    }

    [Fact]
    public void OrderItem_ShouldAddToTableBill()
    {
        // Arrange
        var reservationTime = new DateTime(2025, 6, 15, 18, 0, 0);
        var reservation = _restaurant.CreateScheduledReservation("Smith", 4, reservationTime);
        var burger = _menu.GetItem("Burger")!;
        var salad = _menu.GetItem("Salad")!;
        
        // Act
        _restaurant.OrderItem(reservation.AssignedTable, burger);
        _restaurant.OrderItem(reservation.AssignedTable, salad);
        var bill = _restaurant.CalculateTableBill(reservation.AssignedTable);
        
        // Assert
        Assert.Equal(18.98m, bill);
    }

    [Fact]
    public void CancelItem_ShouldRemoveFromTableOrder()
    {
        // Arrange
        var reservationTime = new DateTime(2025, 6, 15, 18, 0, 0);
        var reservation = _restaurant.CreateScheduledReservation("Smith", 4, reservationTime);
        var burger = _menu.GetItem("Burger")!;
        
        _restaurant.OrderItem(reservation.AssignedTable, burger);
        
        // Act
        _restaurant.CancelItem(reservation.AssignedTable, burger);
        var bill = _restaurant.CalculateTableBill(reservation.AssignedTable);
        
        // Assert
        Assert.Equal(0m, bill);
    }

    [Fact]
    public void OrderItem_ShouldTransitionStatusToSentToKitchen()
    {
        // Arrange
        var reservationTime = new DateTime(2025, 6, 15, 18, 0, 0);
        var reservation = _restaurant.CreateScheduledReservation("Smith", 4, reservationTime);
        var burger = _menu.GetItem("Burger")!;
        
        // Act
        _restaurant.OrderItem(reservation.AssignedTable, burger);
        
        // Assert
        var orderedItems = reservation.AssignedTable.OrderedItems[burger];
        Assert.Single(orderedItems);
        Assert.Equal(OrderStatus.SentToKitchen, orderedItems[0].Status);
    }

    [Fact]
    public void DeliverItem_ShouldTransitionStatusToDelivered()
    {
        // Arrange
        var reservationTime = new DateTime(2025, 6, 15, 18, 0, 0);
        var reservation = _restaurant.CreateScheduledReservation("Smith", 4, reservationTime);
        var burger = _menu.GetItem("Burger")!;
        
        _restaurant.OrderItem(reservation.AssignedTable, burger);
        
        // Act
        _restaurant.DeliverItem(reservation.AssignedTable, burger);
        
        // Assert
        var orderedItems = reservation.AssignedTable.OrderedItems[burger];
        Assert.Single(orderedItems);
        Assert.Equal(OrderStatus.Delivered, orderedItems[0].Status);
    }
}

public class MenuTests
{
    [Fact]
    public void AddItem_ShouldBeRetrievable()
    {
        // Arrange
        var menu = new Menu.Menu();
        var item = new MenuItem("Pasta", "Italian pasta", 12.99m, Category.Main);
        
        // Act
        menu.AddItem(item);
        var retrieved = menu.GetItem("Pasta");
        
        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal("Pasta", retrieved.Name);
        Assert.Equal(12.99m, retrieved.Price);
        Assert.Equal(Category.Main, retrieved.Category);
    }

    [Fact]
    public void GetItem_NonExistent_ShouldReturnNull()
    {
        // Arrange
        var menu = new Menu.Menu();
        
        // Act
        var result = menu.GetItem("NonExistent");
        
        // Assert
        Assert.Null(result);
    }
}

public class TableTests
{
    [Fact]
    public void Table_ShouldCalculateBillCorrectly()
    {
        // Arrange
        var table = new Table.Table(1, 4);
        var item1 = new MenuItem("Burger", "Beef burger", 10.00m, Category.Main);
        var item2 = new MenuItem("Drink", "Soda", 2.50m, Category.Appetizer);
        
        // Act
        table.AddOrder(item1, 2);
        table.AddOrder(item2, 3);
        
        // Assert
        Assert.Equal(27.50m, table.CalculateBillAmount());
    }

    [Fact]
    public void Table_IsAvailableAt_ShouldReturnTrueWhenNoReservation()
    {
        // Arrange
        var table = new Table.Table(1, 4);
        var time = new DateTime(2025, 6, 15, 18, 0, 0);
        
        // Act & Assert
        Assert.True(table.IsAvailableAt(time));
    }
}

public class OrderItemTests
{
    [Fact]
    public void OrderItem_ShouldTransitionThroughStates()
    {
        // Arrange
        var menuItem = new MenuItem("Burger", "Beef burger", 10.00m, Category.Main);
        var orderItem = new OrderItem(menuItem);
        
        // Assert initial state
        Assert.Equal(OrderStatus.Pending, orderItem.Status);
        
        // Act - send to kitchen
        orderItem.SendToKitchen();
        Assert.Equal(OrderStatus.SentToKitchen, orderItem.Status);
        
        // Act - deliver
        orderItem.DeliverToCustomer();
        Assert.Equal(OrderStatus.Delivered, orderItem.Status);
    }

    [Fact]
    public void OrderItem_Cancel_ShouldWorkFromPendingOrSentToKitchen()
    {
        // Arrange
        var menuItem = new MenuItem("Burger", "Beef burger", 10.00m, Category.Main);
        
        // Test cancel from pending
        var orderItem1 = new OrderItem(menuItem);
        orderItem1.Cancel();
        Assert.Equal(OrderStatus.Canceled, orderItem1.Status);
        
        // Test cancel from sent to kitchen
        var orderItem2 = new OrderItem(menuItem);
        orderItem2.SendToKitchen();
        orderItem2.Cancel();
        Assert.Equal(OrderStatus.Canceled, orderItem2.Status);
    }

    [Fact]
    public void OrderItem_Cancel_ShouldNotWorkAfterDelivered()
    {
        // Arrange
        var menuItem = new MenuItem("Burger", "Beef burger", 10.00m, Category.Main);
        var orderItem = new OrderItem(menuItem);
        
        orderItem.SendToKitchen();
        orderItem.DeliverToCustomer();
        
        // Act
        orderItem.Cancel();
        
        // Assert - should still be delivered
        Assert.Equal(OrderStatus.Delivered, orderItem.Status);
    }
}

public class CommandPatternTests
{
    [Fact]
    public void SendToKitchenCommand_ShouldUpdateStatus()
    {
        // Arrange
        var menuItem = new MenuItem("Burger", "Beef burger", 10.00m, Category.Main);
        var orderItem = new OrderItem(menuItem);
        var command = new SendToKitchenCommand(orderItem);
        
        // Act
        command.Execute();
        
        // Assert
        Assert.Equal(OrderStatus.SentToKitchen, orderItem.Status);
    }

    [Fact]
    public void OrderManager_ShouldExecuteMultipleCommands()
    {
        // Arrange
        var menuItem = new MenuItem("Burger", "Beef burger", 10.00m, Category.Main);
        var orderItem1 = new OrderItem(menuItem);
        var orderItem2 = new OrderItem(menuItem);
        
        var orderManager = new OrderManager();
        orderManager.AddCommand(new SendToKitchenCommand(orderItem1));
        orderManager.AddCommand(new SendToKitchenCommand(orderItem2));
        
        // Act
        orderManager.ExecuteCommands();
        
        // Assert
        Assert.Equal(OrderStatus.SentToKitchen, orderItem1.Status);
        Assert.Equal(OrderStatus.SentToKitchen, orderItem2.Status);
    }
}

public class LayoutTests
{
    [Fact]
    public void FindAvailableTable_ShouldReturnSmallestFit()
    {
        // Arrange
        var layout = new Layout([2, 4, 6, 8]);
        var time = new DateTime(2025, 6, 15, 18, 0, 0);
        
        // Act - looking for 3 people, should get table with capacity 4
        var table = layout.FindAvailableTable(3, time);
        
        // Assert
        Assert.NotNull(table);
        Assert.True(table.Capacity >= 3);
    }

    [Fact]
    public void FindAvailableTable_ShouldReturnNullWhenNoFit()
    {
        // Arrange
        var layout = new Layout([2, 4, 6]);
        var time = new DateTime(2025, 6, 15, 18, 0, 0);
        
        // Act - looking for 10 people, no table fits
        var table = layout.FindAvailableTable(10, time);
        
        // Assert
        Assert.Null(table);
    }
}
