using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.Services
{
    public class BookingService
    {
        private readonly ApplicationDbContext context;

        public BookingService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> IsRoomAvailable(int roomId, DateTime startTime, DateTime endTime)
        {
            var overlappingBooking = await context.Bookings
                .FirstOrDefaultAsync(b => b.ConferenceRoomId == roomId &&
                                           b.StartTime < endTime &&
                                           b.EndTime > startTime);
            return overlappingBooking == null;
        }
        public async Task AddBookingAsync(Booking booking)
        {
            if(booking.StartTime >= booking.EndTime)
            {
                throw new ArgumentException("EndTime must be after StartTime");
            }
            if(booking.StartTime < DateTime.Now)
            {
                throw new ArgumentException("StartTime must be in the future");
            }
            if(!await IsRoomAvailable(booking.ConferenceRoomId, booking.StartTime, booking.EndTime))
            {
                throw new InvalidOperationException("The conference room is not available for the selected time slot.");
            }

            context.Bookings.Add(booking);
            await context.SaveChangesAsync();
        }

        public async Task RemoveBookingAsync(int id)
        {
            var booking = await context.Bookings.FindAsync(id);
            if (booking != null)
            {
                context.Bookings.Remove(booking);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await context.Bookings.FindAsync(id);
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await context.Bookings.ToListAsync();
        }
    }
}
