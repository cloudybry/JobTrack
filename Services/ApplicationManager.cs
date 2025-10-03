using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using JobTrack.Models;

namespace JobTrack.Services
{
    public class ApplicationManager
    {
        private const string DataPath = "Data/applications.json";
        private List<JobApplication> applications = new();

        public ApplicationManager()
        {
            Load();
        }

        public void Add(JobApplication app)
        {
            applications.Add(app);
            Save();
        }

        public void List()
        {
            if (applications.Count == 0)
            {
                Console.WriteLine("📭 No applications found.");
                return;
            }

            foreach (var app in applications)
            {
                Console.WriteLine($"{app.Company ?? "N/A"} | {app.Role ?? "N/A"} | {app.DateApplied:yyyy-MM-dd} | {app.Status ?? "N/A"}");
            }
        }

        public void UpdateStatus(string? company, string? newStatus)
        {
            if (string.IsNullOrWhiteSpace(company) || string.IsNullOrWhiteSpace(newStatus))
            {
                Console.WriteLine("❌ Company or status cannot be empty.");
                return;
            }

            var app = applications.FirstOrDefault(a => a.Company?.Equals(company, StringComparison.OrdinalIgnoreCase) == true);
            if (app != null)
            {
                app.Status = newStatus;
                Save();
                Console.WriteLine($"✅ Status updated to '{newStatus}' for {company}.");
            }
            else
            {
                Console.WriteLine($"❌ No application found for '{company}'.");
            }
        }

        public void Search(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("❌ Keyword cannot be empty.");
                return;
            }

            var results = applications.Where(a =>
                a.Company?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                a.Role?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                a.Status?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true).ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("🔍 No matches found.");
                return;
            }

            foreach (var app in results)
            {
                Console.WriteLine($"{app.Company ?? "N/A"} | {app.Role ?? "N/A"} | {app.Status ?? "N/A"}");
            }
        }

        public void ShowStats()
        {
            int total = applications.Count;
            int interviews = applications.Count(a => a.Status?.ToLower().Contains("interview") == true);
            int offers = applications.Count(a => a.Status?.ToLower().Contains("offer") == true);
            int rejections = applications.Count(a => a.Status?.ToLower().Contains("reject") == true);

            Console.WriteLine($"📊 Total: {total} | Interviews: {interviews} | Offers: {offers} | Rejections: {rejections}");
        }

        public List<JobApplication> GetAll()
        {
            return applications;
        }

        public void ExportToCsv(string path = "exports/applications.csv")
        {
            Directory.CreateDirectory("exports");
            var sb = new StringBuilder();
            sb.AppendLine("Company,Role,DateApplied,Status,Notes");

            foreach (var app in applications)
            {
                sb.AppendLine($"\"{app.Company ?? ""}\",\"{app.Role ?? ""}\",\"{app.DateApplied:yyyy-MM-dd}\",\"{app.Status ?? ""}\",\"{app.Notes ?? ""}\"");
            }

            File.WriteAllText(path, sb.ToString());
            Console.WriteLine($"📁 Exported to {path}");
        }

        public void ExportToMarkdown(string path = "exports/applications.md")
        {
            Directory.CreateDirectory("exports");
            var sb = new StringBuilder();
            sb.AppendLine("# 📄 Job Applications\n");

            foreach (var app in applications)
            {
                sb.AppendLine($"### {app.Company ?? "Unknown"} — {app.Role ?? "Unknown"}");
                sb.AppendLine($"- **Date Applied**: {app.DateApplied:yyyy-MM-dd}");
                sb.AppendLine($"- **Status**: {app.Status ?? "N/A"}");
                sb.AppendLine($"- **Notes**: {app.Notes ?? "N/A"}\n");
            }

            File.WriteAllText(path, sb.ToString());
            Console.WriteLine($"📁 Exported to {path}");
        }

        private void Load()
        {
            if (File.Exists(DataPath))
            {
                var json = File.ReadAllText(DataPath);
                applications = JsonSerializer.Deserialize<List<JobApplication>>(json) ?? new();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(applications, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory("Data");
            File.WriteAllText(DataPath, json);
        }
    }
}