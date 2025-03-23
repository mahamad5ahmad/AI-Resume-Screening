using DocumentFormat.OpenXml.Packaging;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeScreeningSystem.Data;
using ResumeScreeningSystem.Models;
using System.Text.RegularExpressions;
using System.Text;


namespace ResumeScreeningSystem.Controllers
{
    [Route("api/resumes")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ResumeController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("upload")]
        [Authorize]  // Only authenticated recruiters can upload resumes
        public async Task<IActionResult> UploadResume( IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");
            Console.WriteLine("------------------------------jj-----------------");
            Console.WriteLine(_environment.WebRootPath);
            string filePath = System.IO.Path.Combine(_environment.WebRootPath, "uploads", file.FileName);

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath)); // Ensure directory exists

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Extract text from the resume
            string extractedText = file.FileName.EndsWith(".pdf") ? ExtractTextFromPdf(filePath) : ExtractTextFromDocx(filePath);

            // Parse resume details
            Resume parsedResume = ParseResume(extractedText);
            parsedResume.FilePath = filePath;

            // Save to database
            _context.Resumes.Add(parsedResume);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Resume uploaded successfully!", resume = parsedResume });
        }

        private string ExtractTextFromPdf(string filePath)
        {
            using (PdfReader reader = new PdfReader(filePath))
            {
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return text.ToString();
            }
        }

        private string ExtractTextFromDocx(string filePath)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
            {
                return doc.MainDocumentPart.Document.Body.InnerText;
            }
        }

        private Resume ParseResume(string text)
        {
            return new Resume
            {
                CandidateName = ExtractName(text),
                Email = ExtractEmail(text),
                Phone = ExtractPhoneNumber(text),
                Skills = ExtractSkills(text),
                Education = ExtractEducation(text),
                Experience = ExtractExperience(text)
            };
        }

        private string ExtractName(string text)
        {
            var lines = text.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            return lines.Count > 0 ? lines[0].Trim() : "Unknown";
        }

        private string ExtractEmail(string text)
        {
            var match = Regex.Match(text, @"[\w\.-]+@[\w\.-]+\.\w+");
            return match.Success ? match.Value : "Not Found";
        }

        private string ExtractPhoneNumber(string text)
        {
            var match = Regex.Match(text, @"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}");
            return match.Success ? match.Value : "Not Found";
        }

        private string ExtractSkills(string text)
        {
            string[] commonSkills = { "C#", "Python", "Java", "SQL", "JavaScript", "AI", "Machine Learning", "ASP.NET" };
            var foundSkills = commonSkills.Where(skill => text.Contains(skill, StringComparison.OrdinalIgnoreCase));
            return string.Join(", ", foundSkills);
        }

        private string ExtractEducation(string text)
        {
            string[] educationKeywords = { "Bachelor", "Master", "PhD", "Degree", "University", "College" };
            var educationLines = text.Split('\n').Where(line => educationKeywords.Any(k => line.Contains(k, StringComparison.OrdinalIgnoreCase)));
            return string.Join(", ", educationLines);
        }

        private string ExtractExperience(string text)
        {
            string[] experienceKeywords = { "years of experience", "worked at", "employment history", "experience in" };
            var experienceLines = text.Split('\n').Where(line => experienceKeywords.Any(k => line.Contains(k, StringComparison.OrdinalIgnoreCase)));
            return string.Join(", ", experienceLines);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetResumes()
        {
            return Ok(_context.Resumes.ToList());
        }
    }
}
