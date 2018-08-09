namespace DAL.Models
{
    public class UserDO
    {
        public long UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public int Role { get; set; }

        public string RoleName { get; set; }
    }
}
