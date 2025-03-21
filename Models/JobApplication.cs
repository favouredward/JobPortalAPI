using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalAPI.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public Job Job { get; set; }

        [Required]
        public string JobSeekerId { get; set; }  // User ID of the applicant

        public string CVUrl { get; set; }  // CV/Resume upload

        [Required]
        public string CoverLetter { get; set; }

        public string Status { get; set; } = "Pending"; // Default: Pending, Can be: Shortlisted, Rejected

        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;
    }
}
