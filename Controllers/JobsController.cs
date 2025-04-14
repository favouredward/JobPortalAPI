using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobPortalAPI.Models;
using JobPortalAPI.Services;
using System.Threading.Tasks;

namespace JobPortalAPI.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobService _jobService;

        public JobsController(JobService jobService)
        {
            _jobService = jobService;
        }

        // ✅ Create a Job (Only Employers can post jobs)
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob([FromBody] Job job)
        {
            if (job == null)
                return BadRequest("Invalid job data.");

            var createdJob = await _jobService.CreateJobAsync(job);
            return CreatedAtAction(nameof(GetJobById), new { id = createdJob.Id }, createdJob);
        }

        // ✅ Get All Jobs (Anyone can view)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }

        // ✅ Get Job By ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
                return NotFound("Job not found.");

            return Ok(job);
        }

        // ✅ Update a Job (Only the Employer who posted it can edit)
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] Job updatedJob)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
                return NotFound("Job not found.");

            updatedJob.Id = id; // Ensure the ID is consistent
            var updated = await _jobService.CreateJobAsync(updatedJob);
            return Ok(updated);
        }

        // ✅ Delete a Job (Only Admins can delete jobs)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var result = await _jobService.DeleteJobAsync(id);
            if (!result)
                return NotFound("Job not found.");

            return Ok("Job deleted successfully.");
        }
    }
}
