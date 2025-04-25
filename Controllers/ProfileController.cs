using Microsoft.AspNetCore.Authorization; // For [Authorize]
using Microsoft.AspNetCore.Mvc; // For [ApiController], ControllerBase, IActionResult, [HttpGet], [HttpPut]
using System.Security.Claims; // For ClaimTypes
using System.Threading.Tasks; // For async/await
using JobPortalAPI.Models; // For User model
using JobPortalAPI.Data; // For ApplicationDbContext

namespace JobPortalAPI.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            if (userId == null)
                return Unauthorized("User ID not found.");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] User updatedProfile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            if (userId == null)
                return Unauthorized("User ID not found.");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return NotFound("User not found.");

            // Update user properties
            user.FirstName = updatedProfile.FirstName;
            user.LastName = updatedProfile.LastName;
            user.ProfileImageUrl = updatedProfile.ProfileImageUrl;
            user.CVUrl = updatedProfile.CVUrl;
            user.CompanyName = updatedProfile.CompanyName;
            user.CompanyDescription = updatedProfile.CompanyDescription;

            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
