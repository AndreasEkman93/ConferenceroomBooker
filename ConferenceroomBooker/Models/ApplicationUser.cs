using System.ComponentModel.DataAnnotations;

namespace ConferenceroomBooker.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; } =string.Empty;
        [Required]
        [MinLength(3)]
        public string Password { get; set; } =string.Empty;
    }
}
