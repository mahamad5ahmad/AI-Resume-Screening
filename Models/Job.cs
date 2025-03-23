using System.ComponentModel.DataAnnotations;

namespace ResumeScreeningSystem.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string RequiredSkills { get; set; } // Comma-separated skills

        [Required]
        public int MinExperience { get; set; } // Minimum years of experience

        [Required]
        public string EducationRequirement { get; set; } // E.g., "Bachelor, Master"

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}
