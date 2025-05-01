using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;
    }
}
