using System.ComponentModel.DataAnnotations;

namespace ResumeScreeningSystem.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        public string CandidateName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skills { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
