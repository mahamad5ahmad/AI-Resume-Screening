using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScreeningSystem.Data;
using ResumeScreeningSystem.Models;
using ResumeScreeningSystem.Services ;

namespace ResumeScreeningSystem.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly CandidateRankingService _rankingService;

        public JobController(ApplicationDbContext context, CandidateRankingService rankingService)
        {
        _context = context;
        _rankingService = rankingService;
        }


        [HttpPost]
        [Authorize]
        //[Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> PostJob([FromBody] Job job)
        {
            if (job == null)
                return BadRequest("Invalid job details.");

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Job posted successfully!", job });
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetJobs()
        {
            return Ok(_context.Jobs.ToList());
        }

        [HttpGet("{jobId}/ai-ranked-candidates")]
        [Authorize]
        //[Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> GetAIRankedCandidates(int jobId)
        {
            var rankedCandidates = await _rankingService.RankCandidatesForJobAsync(jobId);
            return Ok(rankedCandidates.Select(rc => new { Candidate = rc.Item1, Score = rc.Item2 }));
        }
    }
}
