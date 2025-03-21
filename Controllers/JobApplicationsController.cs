using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.Data;
using JobPortalAPI.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace JobPortalAPI.Controllers
{
    [Route("api/job-applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Apply for a Job (Job Seeker Only)
        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ApplyForJob([FromBody] JobApplication application)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user ID
            application.JobSeekerId = userId;

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetApplicationById), new { id = application.Id }, application);
        }

        // ✅ Get All Applications for a Job (Employer Only)
        [HttpGet("job/{jobId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetApplicationsForJob(int jobId)
        {
            var applications = await _context.JobApplications
                .Where(a => a.JobId == jobId)
                .ToListAsync();

            return Ok(applications);
        }

        // ✅ Get a Single Application by ID (Employer or Applicant)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetApplicationById(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application == null)
                return NotFound("Application not found.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.JobSeekerId != userId && !User.IsInRole("Employer"))
                return Forbid("Access denied.");

            return Ok(application);
        }

        // ✅ Employer Updates Application Status (Shortlist/Reject)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] string status)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application == null)
                return NotFound("Application not found.");

            application.Status = status;
            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();
            return Ok(application);
        }
    }
}
