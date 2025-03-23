using Microsoft.AspNetCore.Identity;

namespace ResumeScreeningSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
