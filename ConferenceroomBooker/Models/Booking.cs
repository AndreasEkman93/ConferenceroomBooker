namespace ConferenceroomBooker.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int ConferenceRoomId { get; set; }
        public int ApplicationUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool DateValidation(DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
            {
                throw new ArgumentException("EndTime must be after StartTime");
            }
            if (startTime < DateTime.Now)
            {
                throw new ArgumentException("StartTime must be in the future");
            }
            if (startTime == endTime)
            {
                throw new ArgumentException("StartTime and EndTime cannot be the same");
            }

            return true;


        }
    }

}
