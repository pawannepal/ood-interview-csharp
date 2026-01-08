using OodInterview.ParkingLot.Fare;
using OodInterview.ParkingLot.Spot;
using OodInterview.ParkingLot.Vehicle;

namespace OodInterview.ParkingLot.Tests;

public class ParkingLotSystemTests
{
    [Fact]
    public void TestVehicleJourney()
    {
        // Set up parking spots
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Medium] = [new RegularSpot(1), new RegularSpot(2)]
        };

        // Initialize parking manager
        var parkingManager = new ParkingManager(availableSpots);

        // Set up fare calculation
        var strategies = new List<IFareStrategy> { new BaseFareStrategy(), new PeakHoursFareStrategy() };
        var fareCalculator = new FareCalculator(strategies);

        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        // Create a test vehicle
        var car = new Car("ABC123");

        // Vehicle enters the parking lot
        var ticket = parkingLot.EnterVehicle(car);
        Assert.NotNull(ticket);
        Assert.Equal(car, ticket.Vehicle);
        Assert.NotNull(ticket.ParkingSpot);

        // Find the vehicle in the parking lot
        var foundSpot = parkingLot.FindVehicleSpot(car);
        Assert.NotNull(foundSpot);
        Assert.Equal(ticket.ParkingSpot, foundSpot);

        // Vehicle leaves the parking lot
        var fare = parkingLot.LeaveVehicle(ticket);
        Assert.NotNull(ticket.ExitTime);
        Assert.True(foundSpot.IsAvailable);
    }

    [Fact]
    public void TestMotorcycleParking()
    {
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Small] = [new CompactSpot(1)]
        };

        var parkingManager = new ParkingManager(availableSpots);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        var motorcycle = new Motorcycle("MOTO123");
        var ticket = parkingLot.EnterVehicle(motorcycle);

        Assert.NotNull(ticket);
        Assert.Equal(VehicleSize.Small, ticket.Vehicle.Size);
    }

    [Fact]
    public void TestTruckParking()
    {
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Large] = [new OversizedSpot(1)]
        };

        var parkingManager = new ParkingManager(availableSpots);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        var truck = new Truck("TRUCK123");
        var ticket = parkingLot.EnterVehicle(truck);

        Assert.NotNull(ticket);
        Assert.Equal(VehicleSize.Large, ticket.Vehicle.Size);
    }

    [Fact]
    public void TestNoSpotAvailable()
    {
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Medium] = [new RegularSpot(1)]
        };

        var parkingManager = new ParkingManager(availableSpots);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        var car1 = new Car("CAR1");
        var car2 = new Car("CAR2");

        var ticket1 = parkingLot.EnterVehicle(car1);
        var ticket2 = parkingLot.EnterVehicle(car2);

        Assert.NotNull(ticket1);
        Assert.Null(ticket2); // No spot available
    }

    [Fact]
    public void TestSpotBecomesAvailableAfterExit()
    {
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Medium] = [new RegularSpot(1)]
        };

        var parkingManager = new ParkingManager(availableSpots);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        var car1 = new Car("CAR1");
        var car2 = new Car("CAR2");

        // First car enters and fills the spot
        var ticket1 = parkingLot.EnterVehicle(car1);
        Assert.NotNull(ticket1);

        // Second car cannot enter
        var ticket2 = parkingLot.EnterVehicle(car2);
        Assert.Null(ticket2);

        // First car leaves
        parkingLot.LeaveVehicle(ticket1);

        // Now second car can enter
        var ticket3 = parkingLot.EnterVehicle(car2);
        Assert.NotNull(ticket3);
    }

    [Fact]
    public void TestSmallVehicleCanParkInLargerSpot()
    {
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Medium] = [new RegularSpot(1)]
        };

        var parkingManager = new ParkingManager(availableSpots);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        var motorcycle = new Motorcycle("MOTO123");
        var ticket = parkingLot.EnterVehicle(motorcycle);

        // Small vehicle can park in medium spot
        Assert.NotNull(ticket);
    }

    [Fact]
    public void TestFareCalculation()
    {
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Medium] = [new RegularSpot(1)]
        };

        var parkingManager = new ParkingManager(availableSpots);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        var car = new Car("ABC123");
        var ticket = parkingLot.EnterVehicle(car);
        Assert.NotNull(ticket);

        // Small delay to ensure duration > 0
        var fare = parkingLot.LeaveVehicle(ticket);
        Assert.NotNull(fare);
        Assert.True(fare >= 0);
    }

    [Fact]
    public void TestMultipleVehicleSizes()
    {
        var availableSpots = new Dictionary<VehicleSize, List<IParkingSpot>>
        {
            [VehicleSize.Small] = [new CompactSpot(1)],
            [VehicleSize.Medium] = [new RegularSpot(2)],
            [VehicleSize.Large] = [new OversizedSpot(3)]
        };

        var parkingManager = new ParkingManager(availableSpots);
        var fareCalculator = new FareCalculator([new BaseFareStrategy()]);
        var parkingLot = new ParkingLotSystem(parkingManager, fareCalculator);

        var motorcycle = new Motorcycle("MOTO1");
        var car = new Car("CAR1");
        var truck = new Truck("TRUCK1");

        var ticketMoto = parkingLot.EnterVehicle(motorcycle);
        var ticketCar = parkingLot.EnterVehicle(car);
        var ticketTruck = parkingLot.EnterVehicle(truck);

        Assert.NotNull(ticketMoto);
        Assert.NotNull(ticketCar);
        Assert.NotNull(ticketTruck);

        Assert.Equal(1, ticketMoto.ParkingSpot.SpotNumber);
        Assert.Equal(2, ticketCar.ParkingSpot.SpotNumber);
        Assert.Equal(3, ticketTruck.ParkingSpot.SpotNumber);
    }
}
