// DTOs/RegisterUserDTO.cs
using JobPortalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // Plain text password for registration

        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
