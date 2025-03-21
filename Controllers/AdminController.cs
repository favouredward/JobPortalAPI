using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.Data;
using JobPortalAPI.Models;
using System.Threading.Tasks;
using System.Linq;

namespace JobPortalAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Restrict all actions to Admin only
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get All Users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // ✅ Activate / Deactivate a User
        [HttpPut("users/{userId}/status")]
        public async Task<IActionResult> UpdateUserStatus(string userId, [FromBody] bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            user.IsActive = isActive;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok($"User {(isActive ? "activated" : "deactivated")} successfully.");
        }

        // ✅ Approve or Reject Job Postings
        [HttpPut("jobs/{jobId}/approve")]
        public async Task<IActionResult> ApproveJob(int jobId, [FromBody] bool isApproved)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
                return NotFound("Job not found.");

            job.IsApproved = isApproved;
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
            return Ok($"Job {(isApproved ? "approved" : "rejected")} successfully.");
        }

        // ✅ Get System Statistics
        [HttpGet("stats")]
        public async Task<IActionResult> GetStatistics()
        {
            var userCount = await _context.Users.CountAsync();
            var jobCount = await _context.Jobs.CountAsync();
            var applicationCount = await _context.JobApplications.CountAsync();

            return Ok(new
            {
                TotalUsers = userCount,
                TotalJobs = jobCount,
                TotalApplications = applicationCount
            });
        }
    }
}
