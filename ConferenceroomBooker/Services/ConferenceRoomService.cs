using ConferenceroomBooker.Data;
using ConferenceroomBooker.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.Services
{
    public class ConferenceRoomService
    {
        private readonly ApplicationDbContext context;

        public ConferenceRoomService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddConferenceRoomAsync(ConferenceRoom conferenceRoom)
        {
            await context.ConferenceRooms.AddAsync(conferenceRoom);
            await context.SaveChangesAsync();
        }

        public void DeleteConferenceRoomAsync(int id)
        {
            var conferenceRoom = context.ConferenceRooms.Find(id);
            if (conferenceRoom != null)
            {
                context.ConferenceRooms.Remove(conferenceRoom);
                context.SaveChanges();
            }
        }

        public async Task<List<ConferenceRoom>> GetAllAsync()
        {
            return await context.ConferenceRooms.ToListAsync();
        }

        public async Task<ConferenceRoom?> GetConferenceRoomByIdAsync(int id)
        {
            return await context.ConferenceRooms.FindAsync(id);
        }

        public async Task UpdateAsync(ConferenceRoom room)
        {
            context.ConferenceRooms.Update(room);
            await context.SaveChangesAsync();
        }
    }
}
