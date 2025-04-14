using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobPortalAPI.Models;
using JobPortalAPI.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalAPI.Controllers
{
    [Route("api/job-applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public JobApplicationsController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        // ✅ Apply for a Job (Job Seeker Only)
        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> ApplyForJob([FromBody] JobApplication application)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user ID
            if (userId == null)
            {
                return Unauthorized("User ID not found.");
            }
            application.JobSeekerId = userId;

            var createdApplication = await _applicationService.SubmitApplicationAsync(application);
            return CreatedAtAction(nameof(GetApplicationById), new { id = createdApplication.Id }, createdApplication);
        }

        // ✅ Get All Applications for a Job (Employer Only)
        [HttpGet("job/{jobId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetApplicationsForJob(int jobId)
        {
            var applications = await _applicationService.GetApplicationsByJobIdAsync(jobId);
            return Ok(applications);
        }

        // ✅ Get a Single Application by ID (Employer or Applicant)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetApplicationById(int id)
        {
            var application = await _applicationService.GetApplicationsByJobIdAsync(id);
            if (application == null)
                return NotFound("Application not found.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (application.FirstOrDefault()?.JobSeekerId != userId && !User.IsInRole("Employer"))
                return Forbid("Access denied.");

            return Ok(application);
        }

        // ✅ Employer Updates Application Status (Shortlist/Reject)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] string status)
        {
            var application = await _applicationService.GetApplicationsByJobIdAsync(id);
            if (application == null)
                return NotFound("Application not found.");

            application.FirstOrDefault().Status = status;
            return Ok(application);
        }
    }
}
