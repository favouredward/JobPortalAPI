// Models/JobPost.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortalAPI.Models
{
    public enum JobStatus { Open, Closed }

    public class Job
    {
        public int Id { get; set; }

        [Required]
        public int EmployerId { get; set; } // FK to User

        [Required]
        public string? Title { get; set; }

        public string Company { get; set; }
        public string Location { get; set; }

        public string? Description { get; set; }

        public bool IsOpen { get; set; } = true;

        public DateTime Deadline { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false; // Default: Not approved

    }
}
