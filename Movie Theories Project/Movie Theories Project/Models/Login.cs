using System.ComponentModel.DataAnnotations;

namespace Movie_Theories_Project.Models
{
    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}