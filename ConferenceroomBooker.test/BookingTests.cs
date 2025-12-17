using System.Threading.Tasks;
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
        public async Task TestBookingCreate()
        {
        }

        [Fact]
        public async Task TestBookingAddOverlappingToDatabaseAndRetrieve()
        {
            // Arrange
            var booking = new Booking
            {
                ApplicationUserId = 1,
                ConferenceRoomId = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };

            // Act
            await bookingService.AddBookingAsync(booking);
            var retrievedBooking = await bookingService.GetBookingByIdAsync(booking.Id);
            // Assert
            Assert.NotNull(retrievedBooking);
            Assert.Equal(booking.ApplicationUserId, retrievedBooking.ApplicationUserId);
            Assert.Equal(booking.ConferenceRoomId, retrievedBooking.ConferenceRoomId);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await bookingService.AddBookingAsync(booking);
            });
            Assert.Equal("The conference room is not available for the selected time slot.", ex.Message);
        }

        [Fact]
        public async Task TestBookingAddtoDatabaseAndRemove()
        {
            // Arrange
            var booking = new Booking
            {
                ApplicationUserId = 1,
                ConferenceRoomId = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };
            await bookingService.AddBookingAsync(booking);

            // Act
            await bookingService.RemoveBookingAsync(booking.Id);
            var retrievedBooking = await context.Bookings.FindAsync(booking.Id);

            // Assert
            Assert.Null(retrievedBooking);
        }

        [Fact]
        public async Task TestBookingGetAllAsync()
        {
            // Arrange
            List<Booking> bookings = new List<Booking>
            {
                new Booking
                {
                    ApplicationUserId = 1,
                    ConferenceRoomId = 1,
                    StartTime = DateTime.Now.AddHours(1),
                    EndTime = DateTime.Now.AddHours(2)
                },
                new Booking
                {
                    ApplicationUserId = 2,
                    ConferenceRoomId = 1,
                    StartTime = DateTime.Now.AddHours(3),
                    EndTime = DateTime.Now.AddHours(4)
                }
            };
            await bookingService.AddBookingAsync(bookings[0]);
            await bookingService.AddBookingAsync(bookings[1]);

            // Act
            var retrievedBookings = await bookingService.GetAllBookingsAsync();

            // Assert
            Assert.Equal(2, retrievedBookings.Count);
            Assert.Equal(bookings[0].ApplicationUserId, retrievedBookings[0].ApplicationUserId);
            Assert.Equal(bookings[1].ApplicationUserId, retrievedBookings[1].ApplicationUserId);
        }

        [Fact]
        public async Task TestBookingCheckAvailability()
        {
            // Arrange
            var booking = new Booking
            {
                ApplicationUserId = 1,
                ConferenceRoomId = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };
            await bookingService.AddBookingAsync(booking);
            // Act
            var isAvailable = await bookingService.IsRoomAvailable(1, DateTime.Now.AddHours(3), DateTime.Now.AddHours(4));
            var isNotAvailable = await bookingService.IsRoomAvailable(1, DateTime.Now.AddHours(1.5), DateTime.Now.AddHours(2.5));
            // Assert
            Assert.True(isAvailable);
            Assert.False(isNotAvailable);
        }

        [Fact]
        public async Task TestBookingGetAllByConferenceRoomIdAsync()
        {
            // Arrange
            var booking1 = new Booking
            {
                ApplicationUserId = 1,
                ConferenceRoomId = 1,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            };
            var booking2 = new Booking
            {
                ApplicationUserId = 2,
                ConferenceRoomId = 2,
                StartTime = DateTime.Now.AddHours(3),
                EndTime = DateTime.Now.AddHours(4)
            };
            await bookingService.AddBookingAsync(booking1);
            await bookingService.AddBookingAsync(booking2);
            // Act
            var retrievedBooking1 = await bookingService.GetBookingsForConferenceRoomAsync(booking1.ConferenceRoomId);
            var retrievedBooking2 = await bookingService.GetBookingsForConferenceRoomAsync(booking2.ConferenceRoomId);

            // Assert
            Assert.Equal(1, retrievedBooking1[0].ConferenceRoomId);
            Assert.Equal(2, retrievedBooking2[0].ConferenceRoomId);
        }
    }
}

//Enhetstester:
// Testa logiken för att skapa bokningar.
// Testa att överlappande bokningar nekas.
// Testa tillgänglighetskontrollen.
//Integrationstester:
// Testa att bokningar sparas korrekt i databasen.
// Testa att bokningar kan hämtas från databasen.
// Testa att systemet fungerar med en riktig databas (SQLite eller SQL Server).
