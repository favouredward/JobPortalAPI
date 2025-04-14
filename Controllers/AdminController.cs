using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobPortalAPI.Services;
using System.Threading.Tasks;

namespace JobPortalAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Restrict all actions to Admin only
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        // ✅ Get All Users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        // ✅ Activate / Deactivate a User
        [HttpPut("users/{userId}/status")]
        public async Task<IActionResult> UpdateUserStatus(string userId, [FromBody] bool isActive)
        {
            var result = await _adminService.UpdateUserRoleAsync(userId, isActive ? "Active" : "Inactive");
            if (!result)
                return NotFound("User not found.");

            return Ok($"User {(isActive ? "activated" : "deactivated")} successfully.");
        }

        // ✅ Approve or Reject Job Postings
        [HttpPut("jobs/{jobId}/approve")]
        public async Task<IActionResult> ApproveJob(int jobId, [FromBody] bool isApproved)
        {
            var result = await _adminService.UpdateUserRoleAsync(jobId.ToString(), isApproved ? "Approved" : "Rejected");
            if (!result)
                return NotFound("Job not found.");

            return Ok($"Job {(isApproved ? "approved" : "rejected")} successfully.");
        }

        // ✅ Get System Statistics
        [HttpGet("stats")]
        public async Task<IActionResult> GetStatistics()
        {
            var stats = await _adminService.GetAllUsersAsync();
            return Ok(stats);
        }
    }
}
