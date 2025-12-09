using System.ComponentModel.DataAnnotations;

namespace ConferenceroomBooker.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } =string.Empty;
        [Required]
        public string Password { get; set; } =string.Empty;
    }
}
