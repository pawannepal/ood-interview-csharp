using OodInterview.ShippingLocker;
using OodInterview.ShippingLocker.Account;
using OodInterview.ShippingLocker.Locker;
using OodInterview.ShippingLocker.Package;

namespace OodInterview.ShippingLocker.Tests;

public class ShippingLockerSystemTests
{
    private readonly LockerManager _lockerManager;
    private readonly Account.Account _testAccount;

    public ShippingLockerSystemTests()
    {
        var lockerConfiguration = new Dictionary<LockerSize, int>
        {
            { LockerSize.Small, 10 },
            { LockerSize.Medium, 10 },
            { LockerSize.Large, 10 }
        };
        var site = new Site(lockerConfiguration);

        var policy = new AccountLockerPolicy(0, 10);
        _testAccount = new Account.Account("testAccount", "testOwnerName", policy);

        var accounts = new Dictionary<string, Account.Account>
        {
            { "testAccount", _testAccount }
        };

        var notificationService = new TestNotificationService();
        _lockerManager = new LockerManager(site, accounts, notificationService);
    }

    [Fact]
    public void AssignPackage_ShouldAssignToSmallLocker()
    {
        // Arrange
        var pkg = new BasicShippingPackage("orderId", _testAccount, 10.00m, 10.00m, 10.00m);

        // Act
        var assignedLocker = _lockerManager.AssignPackage(pkg, DateTime.Now);

        // Assert
        Assert.Equal(LockerSize.Small, assignedLocker.Size);
        Assert.Equal(ShippingStatus.InLocker, pkg.Status);
        Assert.False(assignedLocker.IsAvailable);
    }

    [Fact]
    public void AssignPackage_MediumPackage_ShouldAssignToMediumLocker()
    {
        // Arrange
        var pkg = new BasicShippingPackage("orderId", _testAccount, 15.00m, 15.00m, 15.00m);

        // Act
        var assignedLocker = _lockerManager.AssignPackage(pkg, DateTime.Now);

        // Assert
        Assert.Equal(LockerSize.Medium, assignedLocker.Size);
        Assert.Equal(ShippingStatus.InLocker, pkg.Status);
    }

    [Fact]
    public void AssignPackage_LargePackage_ShouldAssignToLargeLocker()
    {
        // Arrange
        var pkg = new BasicShippingPackage("orderId", _testAccount, 25.00m, 25.00m, 25.00m);

        // Act
        var assignedLocker = _lockerManager.AssignPackage(pkg, DateTime.Now);

        // Assert
        Assert.Equal(LockerSize.Large, assignedLocker.Size);
        Assert.Equal(ShippingStatus.InLocker, pkg.Status);
    }

    [Fact]
    public void PickUpPackage_ShouldReleaseLocker()
    {
        // Arrange
        var twoDaysAgo = DateTime.Now.AddDays(-2);
        var pkg = new BasicShippingPackage("orderId", _testAccount, 10.00m, 10.00m, 10.00m);
        var assignedLocker = _lockerManager.AssignPackage(pkg, twoDaysAgo);
        var accessCode = assignedLocker.AccessCode!;

        // Act
        var foundLocker = _lockerManager.PickUpPackage(accessCode);

        // Assert
        Assert.NotNull(foundLocker);
        Assert.True(foundLocker.IsAvailable);
        Assert.Equal(ShippingStatus.Retrieved, pkg.Status);
    }

    [Fact]
    public void PickUpPackage_ShouldCalculateCharges()
    {
        // Arrange - account with 0 free days, $5/day for small locker
        var twoDaysAgo = DateTime.Now.AddDays(-2);
        var pkg = new BasicShippingPackage("orderId", _testAccount, 10.00m, 10.00m, 10.00m);
        var assignedLocker = _lockerManager.AssignPackage(pkg, twoDaysAgo);
        var accessCode = assignedLocker.AccessCode!;

        // Act
        _lockerManager.PickUpPackage(accessCode);

        // Assert - 2 days * $5/day = $10
        Assert.Equal(10.00m, _testAccount.UsageCharges);
    }

    [Fact]
    public void PickUpPackage_WithFreePeriod_ShouldNotChargeForFreeDays()
    {
        // Arrange - account with 1 free day
        var policyWithFreeDays = new AccountLockerPolicy(1, 10);
        var accountWithFreeDays = new Account.Account("freeAccount", "FreeOwner", policyWithFreeDays);
        var accounts = new Dictionary<string, Account.Account>
        {
            { "freeAccount", accountWithFreeDays }
        };
        var site = new Site(new Dictionary<LockerSize, int>
        {
            { LockerSize.Small, 5 }
        });
        var manager = new LockerManager(site, accounts, new TestNotificationService());

        var twoDaysAgo = DateTime.Now.AddDays(-2);
        var pkg = new BasicShippingPackage("orderId", accountWithFreeDays, 10.00m, 10.00m, 10.00m);
        var locker = manager.AssignPackage(pkg, twoDaysAgo);
        var accessCode = locker.AccessCode!;

        // Act
        manager.PickUpPackage(accessCode);

        // Assert - 2 days - 1 free day = 1 chargeable day * $5 = $5
        Assert.Equal(5.00m, accountWithFreeDays.UsageCharges);
    }

    [Fact]
    public void GetAccount_ShouldReturnCorrectAccount()
    {
        // Act
        var account = _lockerManager.GetAccount("testAccount");

        // Assert
        Assert.NotNull(account);
        Assert.Equal("testAccount", account.AccountId);
        Assert.Equal("testOwnerName", account.OwnerName);
    }

    [Fact]
    public void GetAccount_NonExistent_ShouldReturnNull()
    {
        // Act
        var account = _lockerManager.GetAccount("nonExistent");

        // Assert
        Assert.Null(account);
    }
}

public class LockerTests
{
    [Fact]
    public void Locker_InitialState_ShouldBeAvailable()
    {
        // Arrange & Act
        var locker = new Locker.Locker(LockerSize.Small);

        // Assert
        Assert.True(locker.IsAvailable);
        Assert.Null(locker.Package);
        Assert.Null(locker.AccessCode);
    }

    [Fact]
    public void AssignPackage_ShouldGenerateAccessCode()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        var locker = new Locker.Locker(LockerSize.Small);
        var pkg = new BasicShippingPackage("orderId", account, 5.00m, 5.00m, 5.00m);

        // Act
        locker.AssignPackage(pkg, DateTime.Now);

        // Assert
        Assert.False(locker.IsAvailable);
        Assert.NotNull(locker.AccessCode);
        Assert.Equal(6, locker.AccessCode.Length);
        Assert.Equal(pkg, locker.Package);
    }

    [Fact]
    public void ReleaseLocker_ShouldMakeAvailable()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        var locker = new Locker.Locker(LockerSize.Medium);
        var pkg = new BasicShippingPackage("orderId", account, 15.00m, 15.00m, 15.00m);
        locker.AssignPackage(pkg, DateTime.Now);

        // Act
        locker.ReleaseLocker();

        // Assert
        Assert.True(locker.IsAvailable);
        Assert.Null(locker.Package);
        Assert.Null(locker.AccessCode);
    }

    [Fact]
    public void CheckAccessCode_ShouldReturnTrueForMatchingCode()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        var locker = new Locker.Locker(LockerSize.Small);
        var pkg = new BasicShippingPackage("orderId", account, 5.00m, 5.00m, 5.00m);
        locker.AssignPackage(pkg, DateTime.Now);
        var code = locker.AccessCode!;

        // Act & Assert
        Assert.True(locker.CheckAccessCode(code));
        Assert.False(locker.CheckAccessCode("wrong"));
    }
}

public class BasicShippingPackageTests
{
    [Fact]
    public void GetLockerSize_SmallPackage_ShouldReturnSmall()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        var pkg = new BasicShippingPackage("orderId", account, 10.00m, 10.00m, 10.00m);

        // Act
        var size = pkg.GetLockerSize();

        // Assert
        Assert.Equal(LockerSize.Small, size);
    }

    [Fact]
    public void GetLockerSize_MediumPackage_ShouldReturnMedium()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        var pkg = new BasicShippingPackage("orderId", account, 15.00m, 15.00m, 15.00m);

        // Act
        var size = pkg.GetLockerSize();

        // Assert
        Assert.Equal(LockerSize.Medium, size);
    }

    [Fact]
    public void GetLockerSize_LargePackage_ShouldReturnLarge()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        var pkg = new BasicShippingPackage("orderId", account, 25.00m, 25.00m, 25.00m);

        // Act
        var size = pkg.GetLockerSize();

        // Assert
        Assert.Equal(LockerSize.Large, size);
    }

    [Fact]
    public void GetLockerSize_OversizedPackage_ShouldThrow()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        var pkg = new BasicShippingPackage("orderId", account, 50.00m, 50.00m, 50.00m);

        // Act & Assert
        Assert.Throws<PackageIncompatibleException>(() => pkg.GetLockerSize());
    }
}

public class SiteTests
{
    [Fact]
    public void FindAvailableLocker_ShouldReturnLocker()
    {
        // Arrange
        var site = new Site(new Dictionary<LockerSize, int>
        {
            { LockerSize.Small, 5 },
            { LockerSize.Medium, 3 }
        });

        // Act
        var locker = site.FindAvailableLocker(LockerSize.Small);

        // Assert
        Assert.NotNull(locker);
        Assert.Equal(LockerSize.Small, locker.Size);
    }

    [Fact]
    public void FindAvailableLocker_NoAvailable_ShouldReturnNull()
    {
        // Arrange
        var site = new Site(new Dictionary<LockerSize, int>
        {
            { LockerSize.Large, 1 }
        });

        // Act - request a size that doesn't exist
        var locker = site.FindAvailableLocker(LockerSize.Small);

        // Assert
        Assert.Null(locker);
    }

    [Fact]
    public void PlacePackage_NoLocker_ShouldThrow()
    {
        // Arrange
        var site = new Site(new Dictionary<LockerSize, int>
        {
            { LockerSize.Small, 1 }
        });
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);
        
        // Fill the only small locker
        var pkg1 = new BasicShippingPackage("order1", account, 10.00m, 10.00m, 10.00m);
        site.PlacePackage(pkg1, DateTime.Now);

        // Try to place another small package
        var pkg2 = new BasicShippingPackage("order2", account, 10.00m, 10.00m, 10.00m);

        // Act & Assert
        Assert.Throws<NoLockerAvailableException>(() => site.PlacePackage(pkg2, DateTime.Now));
    }
}

public class AccountTests
{
    [Fact]
    public void Account_ShouldAccumulateCharges()
    {
        // Arrange
        var policy = new AccountLockerPolicy(0, 10);
        var account = new Account.Account("test", "Test Owner", policy);

        // Act
        account.AddUsageCharge(5.00m);
        account.AddUsageCharge(10.00m);

        // Assert
        Assert.Equal(15.00m, account.UsageCharges);
    }

    [Fact]
    public void AccountLockerPolicy_ShouldHaveCorrectValues()
    {
        // Arrange & Act
        var policy = new AccountLockerPolicy(3, 14);

        // Assert
        Assert.Equal(3, policy.FreePeriodDays);
        Assert.Equal(14, policy.MaximumPeriodDays);
    }
}

public class LockerSizeExtensionsTests
{
    [Theory]
    [InlineData(LockerSize.Small, 5.00)]
    [InlineData(LockerSize.Medium, 10.00)]
    [InlineData(LockerSize.Large, 15.00)]
    public void GetDailyCharge_ShouldReturnCorrectValue(LockerSize size, decimal expectedCharge)
    {
        Assert.Equal(expectedCharge, size.GetDailyCharge());
    }

    [Theory]
    [InlineData(LockerSize.Small, 10.00)]
    [InlineData(LockerSize.Medium, 20.00)]
    [InlineData(LockerSize.Large, 30.00)]
    public void GetDimensions_ShouldReturnCorrectValue(LockerSize size, decimal expectedDimension)
    {
        Assert.Equal(expectedDimension, size.GetWidth());
        Assert.Equal(expectedDimension, size.GetHeight());
        Assert.Equal(expectedDimension, size.GetDepth());
    }
}

/// <summary>
/// Test notification service for testing purposes.
/// </summary>
internal class TestNotificationService : INotificationService
{
    public List<string> SentMessages { get; } = [];

    public void SendNotification(string message, Account.Account user)
    {
        SentMessages.Add($"{user.OwnerName}: {message}");
    }
}
