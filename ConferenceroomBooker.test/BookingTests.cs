using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using ConferenceroomBooker.Services;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.test
{
    public class BookingTests : IDisposable
    {
        private ApplicationDbContext context;
        private BookingService bookingService;

        public BookingTests()
        {
            context = CreateInMemoryDbContext();
            bookingService = new BookingService(context);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        private ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unik databas per test
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void TestBookingCreate()
        {

            // Act
            var booking = new Booking
            {
                ApplicationUserId = 1,
                ConferenceRoomId = 1,
            };
            // Assert
            Assert.NotNull(booking);
        }

        [Fact]
        public async Task TestBookingAddToDatabaseAndRetrieve()
        {
            // Arrange
            var booking = new Booking
            {
                ApplicationUserId = 1,
                ConferenceRoomId = 1,
            };

            // Act
            await bookingService.AddBookingAsync(booking);
            var retrievedBooking = await context.Bookings.FindAsync(booking.Id);
            // Assert
            Assert.NotNull(retrievedBooking);
            Assert.Equal(booking.ApplicationUserId, retrievedBooking.ApplicationUserId);
            Assert.Equal(booking.ConferenceRoomId, retrievedBooking.ConferenceRoomId);
        }

        [Fact]
        public async Task TestBookingAddtoDatabaseAndRemove()
        {
            // Arrange
            var booking = new Booking
            {
                ApplicationUserId = 1,
                ConferenceRoomId = 1,
            };
            await bookingService.AddBookingAsync(booking);

            // Act
            await bookingService.RemoveBookingAsync(booking.Id);
            var retrievedBooking = await context.Bookings.FindAsync(booking.Id);

            // Assert
            Assert.Null(retrievedBooking);
        }
    }
}
