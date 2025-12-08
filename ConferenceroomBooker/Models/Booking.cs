namespace ConferenceroomBooker.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int ConferenceRoomId { get; set; }
        public int ApplicationUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
