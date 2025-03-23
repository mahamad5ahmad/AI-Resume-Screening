using ResumeScreeningSystem.Data;
using ResumeScreeningSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeScreeningSystem.Services
{
    public class CandidateRankingService
    {
        private readonly ApplicationDbContext _context;
        private readonly OpenAIService _openAIService;

        public CandidateRankingService(ApplicationDbContext context, OpenAIService openAIService)
        {
            _context = context;
            _openAIService = openAIService;
        }

        public async Task<List<(Resume, int)>> RankCandidatesForJobAsync(int jobId)
        {
            var job = _context.Jobs.Find(jobId);
            if (job == null) return new List<(Resume, int)>();

            var resumes = _context.Resumes.ToList();
            List<(Resume, int)> rankedCandidates = new List<(Resume, int)>();

            foreach (var resume in resumes)
            {
                int score = await GetAIScore(resume, job);
                rankedCandidates.Add((resume, score));
            }

            return rankedCandidates.OrderByDescending(r => r.Item2).ToList();
        }

        private async Task<int> GetAIScore(Resume resume, Job job)
        {
            string resumeText = $"Name: {resume.CandidateName}\nSkills: {resume.Skills}\nExperience: {resume.Experience}\nEducation: {resume.Education}";
            string jobDescription = $"Title: {job.Title}\nRequired Skills: {job.RequiredSkills}\nMinimum Experience: {job.MinExperience} years\nEducation Required: {job.EducationRequirement}";

            string aiResponse = await _openAIService.AnalyzeResumeAsync(resumeText, jobDescription);

            if (int.TryParse(aiResponse, out int score))
                return score;
            return 0; // Default to 0 if AI response is invalid
        }
    }
}
