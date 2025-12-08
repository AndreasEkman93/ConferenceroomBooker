using ConferenceroomBooker.Data;

namespace ConferenceroomBooker.Services
{
    public class ConferenceRoomService
    {
        private readonly ApplicationDbContext context;

        public ConferenceRoomService(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}
