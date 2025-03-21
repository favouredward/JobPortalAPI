using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortalAPI.Data;
using JobPortalAPI.Models;
using System.Security.Claims;

namespace JobPortalAPI.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Create a Job (Only Employers can post jobs)
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob([FromBody] Job job)
        {
            if (job == null)
                return BadRequest("Invalid job data.");

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, job);
        }

        // ✅ Get All Jobs (Anyone can view)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();
            return Ok(jobs);
        }

        // ✅ Get Job By ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound("Job not found.");

            return Ok(job);
        }

        // ✅ Update a Job (Only the Employer who posted it can edit)
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] Job updatedJob)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound("Job not found.");

            job.Title = updatedJob.Title;
            job.Description = updatedJob.Description;
            job.Company = updatedJob.Company;
            job.Location = updatedJob.Location;
            job.IsOpen = updatedJob.IsOpen;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }

        // ✅ Delete a Job (Only Admins can delete jobs)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound("Job not found.");

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return Ok("Job deleted successfully.");
        }
    }
}
