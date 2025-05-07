using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? CurrentImagePath { get; set; }

        public IFormFile? NewProfileImage { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Must contain at least one uppercase letter and one digit.")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }

    }
}
