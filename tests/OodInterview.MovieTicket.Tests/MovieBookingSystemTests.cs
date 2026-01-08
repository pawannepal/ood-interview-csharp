using OodInterview.MovieTicket.Location;
using OodInterview.MovieTicket.Rate;
using OodInterview.MovieTicket.Showing;

namespace OodInterview.MovieTicket.Tests;

public class MovieBookingSystemTests
{
    [Fact]
    public void TestBrowseAndBuy()
    {
        // Create booking system
        var bookingSystem = new MovieBookingSystem();
        
        // Create a room with a layout of 10x10 seats with a normal rate of $10.00
        var room = new Room("1", new Layout(10, 10));
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                room.Layout.GetSeatByPosition(i, j)!.PricingStrategy = new NormalRate(10.00m);
            }
        }
        
        // Create a cinema with the room
        bookingSystem.AddCinema(new Cinema("Test Cinema", "Test Location"));
        
        // Create a test movie with a test screening in the room
        const int length = 180;
        var movie = new Movie("Test Movie", "Test Description", length);
        var screening = new Screening(movie, room, DateTime.Now, DateTime.Now.AddMinutes(length));
        
        // Add the movie and screening to the booking system
        bookingSystem.AddMovie(movie);
        bookingSystem.AddScreening(movie, screening);
        
        // Test that the movie and screening are in the booking system
        Assert.Single(bookingSystem.AllMovies);
        Assert.Single(bookingSystem.GetScreeningsForMovie(movie));
        
        // Test that the available seats for the screening are correct
        Assert.Equal(100, bookingSystem.GetAvailableSeats(screening).Count);
        
        // Test that the booking system can book a ticket
        bookingSystem.BookTicket(screening, room.Layout.GetSeatByPosition(0, 0)!);
        Assert.Equal(1, bookingSystem.GetTicketCount(screening));
        
        // Test the price of the ticket
        Assert.Equal(10.00m, bookingSystem.GetTicketsForScreening(screening)[0].Price);
    }

    [Fact]
    public void TestMultipleRoomBookings()
    {
        var bookingSystem = new MovieBookingSystem();
        
        // Create rooms with different layouts
        var room1 = new Room("Room1", new Layout(5, 5));
        var room2 = new Room("Room2", new Layout(8, 8));
        
        // Set pricing for room 1 - normal rate
        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < 5; j++)
            {
                room1.Layout.GetSeatByPosition(i, j)!.PricingStrategy = new NormalRate(12.00m);
            }
        }
        
        // Set pricing for room 2 - premium rate
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                room2.Layout.GetSeatByPosition(i, j)!.PricingStrategy = new PremiumRate(18.00m);
            }
        }
        
        // Create cinema and add rooms
        var cinema = new Cinema("Grand Cinema", "Downtown");
        cinema.AddRoom(room1);
        cinema.AddRoom(room2);
        bookingSystem.AddCinema(cinema);
        
        // Create movie and screenings
        var movie = new Movie("Action Movie", "Action", 120);
        var screening1 = new Screening(movie, room1, DateTime.Now, DateTime.Now.AddMinutes(120));
        var screening2 = new Screening(movie, room2, DateTime.Now.AddHours(3), DateTime.Now.AddHours(5));
        
        bookingSystem.AddMovie(movie);
        bookingSystem.AddScreening(movie, screening1);
        bookingSystem.AddScreening(movie, screening2);
        
        // Verify both screenings
        Assert.Equal(2, bookingSystem.GetScreeningsForMovie(movie).Count);
        
        // Verify seat counts
        Assert.Equal(25, bookingSystem.GetAvailableSeats(screening1).Count);
        Assert.Equal(64, bookingSystem.GetAvailableSeats(screening2).Count);
        
        // Book tickets in different rooms
        bookingSystem.BookTicket(screening1, room1.Layout.GetSeatByPosition(0, 0)!);
        bookingSystem.BookTicket(screening2, room2.Layout.GetSeatByPosition(0, 0)!);
        
        // Verify different prices
        Assert.Equal(12.00m, bookingSystem.GetTicketsForScreening(screening1)[0].Price);
        Assert.Equal(18.00m, bookingSystem.GetTicketsForScreening(screening2)[0].Price);
    }

    [Fact]
    public void TestVipPricing()
    {
        var bookingSystem = new MovieBookingSystem();
        
        // Create a room with VIP seats
        var room = new Room("VIP Room", new Layout(3, 3));
        
        // Set VIP pricing for all seats
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                room.Layout.GetSeatByPosition(i, j)!.PricingStrategy = new VipRate(25.00m);
            }
        }
        
        var cinema = new Cinema("Luxury Cinema", "Mall");
        cinema.AddRoom(room);
        bookingSystem.AddCinema(cinema);
        
        var movie = new Movie("Premiere Film", "Drama", 150);
        var screening = new Screening(movie, room, DateTime.Now, DateTime.Now.AddMinutes(150));
        
        bookingSystem.AddMovie(movie);
        bookingSystem.AddScreening(movie, screening);
        
        // Book a VIP seat
        bookingSystem.BookTicket(screening, room.Layout.GetSeatByPosition(1, 1)!);
        
        // Verify VIP price
        Assert.Equal(25.00m, bookingSystem.GetTicketsForScreening(screening)[0].Price);
    }

    [Fact]
    public void TestAvailableSeatsAfterBooking()
    {
        var bookingSystem = new MovieBookingSystem();
        
        var room = new Room("Test Room", new Layout(3, 3));
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                room.Layout.GetSeatByPosition(i, j)!.PricingStrategy = new NormalRate(10.00m);
            }
        }
        
        var movie = new Movie("Test Movie", "Comedy", 90);
        var screening = new Screening(movie, room, DateTime.Now, DateTime.Now.AddMinutes(90));
        
        bookingSystem.AddMovie(movie);
        bookingSystem.AddScreening(movie, screening);
        
        // Initially 9 seats available
        Assert.Equal(9, bookingSystem.GetAvailableSeats(screening).Count);
        
        // Book 3 seats
        bookingSystem.BookTicket(screening, room.Layout.GetSeatByPosition(0, 0)!);
        bookingSystem.BookTicket(screening, room.Layout.GetSeatByPosition(0, 1)!);
        bookingSystem.BookTicket(screening, room.Layout.GetSeatByPosition(0, 2)!);
        
        // Now 6 seats available
        Assert.Equal(6, bookingSystem.GetAvailableSeats(screening).Count);
        Assert.Equal(3, bookingSystem.GetTicketCount(screening));
    }

    [Fact]
    public void TestCinemaWithMultipleRooms()
    {
        var cinema = new Cinema("Multiplex", "City Center");
        
        var room1 = new Room("1", new Layout(5, 5));
        var room2 = new Room("2", new Layout(6, 6));
        var room3 = new Room("3", new Layout(7, 7));
        
        cinema.AddRoom(room1);
        cinema.AddRoom(room2);
        cinema.AddRoom(room3);
        
        Assert.Equal(3, cinema.Rooms.Count);
        Assert.Equal("Multiplex", cinema.Name);
        Assert.Equal("City Center", cinema.Location);
    }

    [Fact]
    public void TestSeatByNumber()
    {
        var layout = new Layout(3, 3);
        
        // Access seat by number (format: "row-column")
        var seat = layout.GetSeatByNumber("1-2");
        Assert.NotNull(seat);
        Assert.Equal("1-2", seat.SeatNumber);
        
        // Same seat accessed by position
        var seatByPosition = layout.GetSeatByPosition(1, 2);
        Assert.Equal(seat, seatByPosition);
    }

    [Fact]
    public void TestMovieDetails()
    {
        var movie = new Movie("Inception", "Sci-Fi", 148);
        
        Assert.Equal("Inception", movie.Title);
        Assert.Equal("Sci-Fi", movie.Genre);
        Assert.Equal(148, movie.DurationInMinutes);
        Assert.Equal(TimeSpan.FromMinutes(148), movie.Duration);
    }
}
