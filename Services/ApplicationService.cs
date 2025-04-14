using JobPortalAPI.Data;
using JobPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Services
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Submit a new application
        public async Task<JobApplication> SubmitApplicationAsync(JobApplication application)
        {
            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        // Get all applications for a job
        public async Task<List<JobApplication>> GetApplicationsByJobIdAsync(int jobId)
        {
            return await _context.JobApplications
                .Where(app => app.JobId == jobId)
                .ToListAsync();
        }

        // Get applications by user
        public async Task<List<JobApplication>> GetApplicationsByUserIdAsync(string userId)
        {
            return await _context.JobApplications
                .Where(app => app.JobSeekerId == userId)
                .ToListAsync();
        }

        // Delete an application
        public async Task<bool> DeleteApplicationAsync(int applicationId)
        {
            var application = await _context.JobApplications.FindAsync(applicationId);
            if (application == null) return false;

            _context.JobApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
