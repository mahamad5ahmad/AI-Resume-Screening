📌 AI-Powered Resume Screening System
🚀 A .NET Core-based system that uses AI/NLP to screen and rank resumes based on job requirements.

🔹 Features
✔️ User Authentication (JWT-based for recruiters)
✔️ Resume Upload & Parsing (PDF/Word extraction)
✔️ Job Posting & Criteria Matching (Define job criteria)
✔️ AI-Based Ranking (Using OpenAI API for candidate scoring)
✔️ Candidate Shortlisting (List top candidates based on AI ranking)
✔️ API Documentation (Swagger/Postman)

🔹 Tech Stack
.NET Core (ASP.NET Web API)
Entity Framework Core (MySQL)
ML.NET / OpenAI API (AI Resume Ranking)
JWT Authentication
Swagger (API Documentation)
🔹 Installation Steps
1️⃣ Clone the Repository
2️⃣ Set Up Database (MySQL)
3️⃣ Install Dependencies
4️⃣ Apply Migrations and add your own openAI ApiKey
5️⃣ Run the API

🔹 API Endpoints

📌 Authentication
POST /api/auth/register → Register recruiter
POST /api/auth/login → Login recruiter

📌 Resumes
POST /api/resumes/upload → Upload and parse resume
GET /api/resumes → Get all resumes

📌 Jobs
POST /api/jobs → Create a job posting
GET /api/jobs/{id}/ai-ranked-candidates → Get AI-ranked candidates
GET /api/jobs/{id}/shortlisted-candidates?threshold=75 → Get shortlisted candidates

📌 API Docs:
Open http://localhost:5000/swagger
