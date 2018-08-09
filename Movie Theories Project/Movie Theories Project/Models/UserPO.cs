using System.ComponentModel.DataAnnotations;

namespace Movie_Theories_Project.Models
{
    public class UserPO
    {
        public long UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The Username field must be inbetween 6 and 50 characters long.") ]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The Password field must be inbetween 6 and 50 characters long.")]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        public string Email { get; set; }

        public int Role { get; set; }

        public string RoleName { get; set; }
    }
}