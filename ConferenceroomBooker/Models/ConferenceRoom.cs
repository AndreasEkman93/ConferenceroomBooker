using System.ComponentModel.DataAnnotations;

namespace ConferenceroomBooker.Models
{
    public class ConferenceRoom
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
