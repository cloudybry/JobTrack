using System;
using JobTrack.Models;
using JobTrack.Services;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var manager = new ApplicationManager();
        var exporter = new Exporter();

        Console.WriteLine("🧾 JobTrack CLI — Track your job applications like a pro");
        Console.WriteLine("Commands: add, list, update, search, stats, export csv, export md, exit");

        while (true)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine()?.Trim().ToLower();

            switch (input)
            {
                case "add":
                    var app = new JobApplication();
                    Console.Write("Company: "); app.Company = Console.ReadLine();
                    Console.Write("Role: "); app.Role = Console.ReadLine();
                    Console.Write("Date Applied (yyyy-mm-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        Console.WriteLine("❌ Invalid date format.");
                        break;
                    }
                    app.DateApplied = date;
                    Console.Write("Status: "); app.Status = Console.ReadLine();
                    Console.Write("Notes: "); app.Notes = Console.ReadLine();
                    manager.Add(app);
                    Console.WriteLine("✅ Application saved.");
                    break;

                case "list":
                    manager.List();
                    break;

                case "update":
                    Console.Write("Company to update: ");
                    var company = Console.ReadLine();
                    Console.Write("New status: ");
                    var status = Console.ReadLine();
                    manager.UpdateStatus(company, status);
                    break;

                case "search":
                    Console.Write("Keyword: ");
                    var keyword = Console.ReadLine();
                    manager.Search(keyword);
                    break;

                case "stats":
                    manager.ShowStats();
                    break;

                case "export csv":
                    exporter.ExportToCsv(manager.GetAll());
                    break;

                case "export md":
                    exporter.ExportToMarkdown(manager.GetAll());
                    break;

                case "exit":
                    Console.WriteLine("👋 Goodbye! Keep applying with confidence.");
                    return;

                default:
                    Console.WriteLine("❓ Unknown command. Try: add, list, update, search, stats, export csv, export md, exit");
                    break;
            }
        }
    }
}