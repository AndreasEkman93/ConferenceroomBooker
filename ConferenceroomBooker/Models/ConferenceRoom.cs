using System.ComponentModel.DataAnnotations;

namespace ConferenceroomBooker.Models
{
    public class ConferenceRoom
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; }=string.Empty;
    }
}
