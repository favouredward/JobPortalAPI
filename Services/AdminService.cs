using JobPortalAPI.Data;
using JobPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all users
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id.ToString(), // Convert int to string
                    Email = user.Email,
                    Role = user.Role.ToString() // Convert enum to string
                })
                .ToListAsync();
        }

        // Update user role
        public async Task<bool> UpdateUserRoleAsync(string userId, string newRole)
        {
            if (!int.TryParse(userId, out var userIdInt)) return false; // Convert string to int

            var user = await _context.Users.FindAsync(userIdInt);
            if (user == null) return false;

            if (!Enum.TryParse<UserRole>(newRole, out var parsedRole)) return false; // Parse string to enum
            user.Role = parsedRole;

            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a user
        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (!int.TryParse(userId, out var userIdInt)) return false; // Convert string to int

            var user = await _context.Users.FindAsync(userIdInt);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
