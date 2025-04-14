// Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models
{
    public enum UserRole { Admin, Employer, JobSeeker }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }  // Stored hashed using bcrypt  

        [Required]
        public UserRole Role { get; set; }

        // Updated fields for profiles (depending on role)  
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true; // Default: Active  
    }
}
