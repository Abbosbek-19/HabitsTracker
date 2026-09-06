using System;
using System.Collections.Generic;
using System.Linq;
using DailyTaskTracker.Domain.Entities;
using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.ConsoleApp;

public class Program
{
    private static readonly List<TaskItem> Tasks = new();
    private static readonly List<Habit> Habits = new();
    private static readonly Guid DefaultUserId = Guid.NewGuid();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Daily Task & Habit Tracker — Phase 1 Engine";

        SeedInitialData();

        bool running = true;
        while (running)
        {
            RenderHeader();
            RenderMenu();

            Console.Write("\n👉 Choose an option (0-7): ");
            string? choice = Console.ReadLine();

            Console.Clear();
            switch (choice?.Trim())
            {
                case "1":
                    AddNewTask();
                    break;
                case "2":
                    ListAllTasks();
                    break;
                case "3":
                    CompleteTask();
                    break;
                case "4":
                    FilterAndSearchTasks();
                    break;
                case "5":
                    ViewHabitsAndStreaks();
                    break;
                case "6":
                    RenderDashboardSummary();
                    break;
                case "7":
                    DemonstrateLinqCapabilities();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("👋 Goodbye! Happy tracking.");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Invalid choice. Press any key to continue.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void SeedInitialData()
    {
        var task1 = new TaskItem("Study C# & .NET Core", "Learn Domain entities, LINQ, and OOP encapsulation", PriorityLevel.Urgent, DefaultUserId, DateTime.UtcNow.AddHours(4), 120);
        var task2 = new TaskItem("Go to the Gym", "Leg day workout session", PriorityLevel.High, DefaultUserId, DateTime.UtcNow.AddHours(2), 60);
        var task3 = new TaskItem("Read 20 pages of Clean Code", "Focus on single responsibility principle", PriorityLevel.Medium, DefaultUserId, DateTime.UtcNow.AddDays(1), 30);
        var task4 = new TaskItem("Drink 2L Water", "Hydration goal", PriorityLevel.Low, DefaultUserId, DateTime.UtcNow.AddHours(8), 10);

        task1.MarkAsCompleted();
        task2.MarkAsCompleted();

        Tasks.AddRange(new[] { task1, task2, task3, task4 });

        var habit1 = new Habit("C# Coding Practice", "Code for at least 45 minutes daily", HabitFrequency.Daily, 1, DefaultUserId);
        var habit2 = new Habit("Gym Workout", "Exercise 4 times per week", HabitFrequency.Weekly, 4, DefaultUserId);

        // Record past 5 days of habit completions
        for (int i = 5; i >= 1; i--)
        {
            habit1.RecordCompletion(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i)));
        }

        Habits.AddRange(new[] { habit1, habit2 });
    }

    private static void RenderHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================================");
        Console.WriteLine("        ⚡ DAILY TASK & HABIT TRACKER — C# CORE ENGINE ⚡       ");
        Console.WriteLine("==================================================================");
        Console.ResetColor();
    }

    private static void RenderMenu()
    {
        Console.WriteLine("\n[1] ➕ Add New Task");
        Console.WriteLine("[2] 📋 List All Tasks");
        Console.WriteLine("[3] ✅ Mark Task as Completed");
        Console.WriteLine("[4] 🔍 Search & Filter Tasks (LINQ)");
        Console.WriteLine("[5] 🔥 View Habits & Calculate Streaks");
        Console.WriteLine("[6] 📊 View Dashboard Summary");
        Console.WriteLine("[7] 🧪 Test LINQ Operations");
        Console.WriteLine("[0] 🚪 Exit Application");
    }

    private static void AddNewTask()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- ➕ ADD NEW TASK ---");
        Console.ResetColor();

        Console.Write("Enter Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Enter Description: ");
        string description = Console.ReadLine() ?? "";

        Console.WriteLine("Select Priority Level:");
        Console.WriteLine("  1) Low  2) Medium  3) High  4) Urgent");
        Console.Write("Choice (default 2): ");
        string priorityInput = Console.ReadLine() ?? "2";
        PriorityLevel priority = priorityInput switch
        {
            "1" => PriorityLevel.Low,
            "3" => PriorityLevel.High,
            "4" => PriorityLevel.Urgent,
            _ => PriorityLevel.Medium
        };

        Console.Write("Enter Estimated Minutes (optional): ");
        int? minutes = int.TryParse(Console.ReadLine(), out int m) ? m : null;

        try
        {
            var newTask = new TaskItem(title, description, priority, DefaultUserId, DateTime.UtcNow.AddDays(1), minutes);
            Tasks.Add(newTask);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ Task '{newTask.Title}' created successfully! (ID: {newTask.Id})");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ Error creating task: {ex.Message}");
            Console.ResetColor();
        }

        PromptContinue();
    }

    private static void ListAllTasks()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- 📋 ALL TASKS ---");
        Console.ResetColor();

        if (!Tasks.Any())
        {
            Console.WriteLine("No tasks found.");
            PromptContinue();
            return;
        }

        int index = 1;
        foreach (var task in Tasks.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.Priority))
        {
            string statusIcon = task.IsCompleted ? "☑" : "☐";
            ConsoleColor color = task.IsCompleted ? ConsoleColor.DarkGray : GetPriorityColor(task.Priority);

            Console.ForegroundColor = color;
            Console.WriteLine($"{index++}. [{statusIcon}] {task.Title} | Priority: {task.Priority} | Est: {task.EstimatedMinutes ?? 0}m | Created: {task.CreatedAtUtc:HH:mm}");
            if (!string.IsNullOrEmpty(task.Description))
            {
                Console.WriteLine($"   └─ {task.Description}");
            }
            Console.ResetColor();
        }

        PromptContinue();
    }

    private static void CompleteTask()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- ✅ MARK TASK AS COMPLETED ---");
        Console.ResetColor();

        var pendingTasks = Tasks.Where(t => !t.IsCompleted).ToList();
        if (!pendingTasks.Any())
        {
            Console.WriteLine("🎉 All tasks are already completed!");
            PromptContinue();
            return;
        }

        for (int i = 0; i < pendingTasks.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {pendingTasks[i].Title} (Priority: {pendingTasks[i].Priority})");
        }

        Console.Write("\nEnter task number to complete: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= pendingTasks.Count)
        {
            var selectedTask = pendingTasks[choice - 1];
            selectedTask.MarkAsCompleted();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✨ Outstanding! Completed task: '{selectedTask.Title}'");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Invalid task selection.");
        }

        PromptContinue();
    }

    private static void FilterAndSearchTasks()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- 🔍 SEARCH & FILTER (LINQ ENGINE) ---");
        Console.ResetColor();

        Console.Write("Enter search keyword (or press Enter to skip): ");
        string query = Console.ReadLine() ?? "";

        // Demonstrating LINQ Deferred Execution and Query Chaining
        IEnumerable<TaskItem> filteredQuery = Tasks;

        if (!string.IsNullOrWhiteSpace(query))
        {
            filteredQuery = filteredQuery.Where(t => 
                t.Title.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                t.Description.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        var results = filteredQuery.OrderByDescending(t => t.Priority).ToList();

        Console.WriteLine($"\nFound {results.Count} matching tasks:");
        foreach (var task in results)
        {
            string status = task.IsCompleted ? "Completed" : "Pending";
            Console.WriteLine($"• [{status}] {task.Title} (Priority: {task.Priority})");
        }

        PromptContinue();
    }

    private static void ViewHabitsAndStreaks()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- 🔥 HABIT TRACKER & STREAK CALCULATOR ---");
        Console.ResetColor();

        foreach (var habit in Habits)
        {
            int currentStreak = CalculateCurrentStreak(habit);
            Console.WriteLine($"\nHabit: {habit.Name}");
            Console.WriteLine($"Description: {habit.Description}");
            Console.WriteLine($"Frequency: {habit.Frequency} | Total Logs: {habit.Completions.Count}");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"🔥 Current Streak: {currentStreak} days!");
            Console.ResetColor();
        }

        PromptContinue();
    }

    /// <summary>
    /// Algorithmic Streak Calculation using LINQ and DateTime logic.
    /// </summary>
    private static int CalculateCurrentStreak(Habit habit)
    {
        var dates = habit.Completions
            .Select(c => c.CompletedDate)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        if (!dates.Any()) return 0;

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        DateOnly yesterday = today.AddDays(-1);

        // If the habit was not completed today or yesterday, streak is zero
        if (dates[0] != today && dates[0] != yesterday)
        {
            return 0;
        }

        int streak = 1;
        for (int i = 0; i < dates.Count - 1; i++)
        {
            if (dates[i].AddDays(-1) == dates[i + 1])
            {
                streak++;
            }
            else
            {
                break;
            }
        }

        return streak;
    }

    private static void RenderDashboardSummary()
    {
        int total = Tasks.Count;
        int completed = Tasks.Count(t => t.IsCompleted);
        int remaining = total - completed;
        double completionPercentage = total > 0 ? (double)completed / total * 100 : 0;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=====================================================");
        Console.WriteLine($" Good morning! 👋  |  {DateTime.Now:D}");
        Console.WriteLine("=====================================================");
        Console.ResetColor();

        Console.WriteLine("\nToday's Progress:");
        RenderProgressBar((int)completionPercentage);
        Console.WriteLine($"\n📊 {completed} / {total} tasks completed ({completionPercentage:F0}%)");

        Console.WriteLine($"\nRemaining urgent/high priority tasks: {Tasks.Count(t => !t.IsCompleted && t.Priority >= PriorityLevel.High)}");

        PromptContinue();
    }

    private static void RenderProgressBar(int percent)
    {
        int totalBlocks = 20;
        int filledBlocks = (int)Math.Round((percent / 100.0) * totalBlocks);

        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(new string('█', filledBlocks));
        Console.ResetColor();
        Console.Write(new string('░', totalBlocks - filledBlocks));
        Console.Write($"] {percent}%");
    }

    private static void DemonstrateLinqCapabilities()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- 🧪 LINQ OPERATORS DEMONSTRATION ---");
        Console.ResetColor();

        // 1. Where()
        var highPriority = Tasks.Where(t => t.Priority >= PriorityLevel.High).ToList();
        Console.WriteLine($"1. Where(Priority >= High): Count = {highPriority.Count}");

        // 2. Select() - Projection
        var taskTitles = Tasks.Select(t => t.Title).ToList();
        Console.WriteLine($"2. Select(Titles): [{string.Join(", ", taskTitles)}]");

        // 3. Count() & Sum()
        int totalEstimatedMinutes = Tasks.Sum(t => t.EstimatedMinutes ?? 0);
        Console.WriteLine($"3. Sum(EstimatedMinutes): {totalEstimatedMinutes} mins total workload");

        // 4. GroupBy()
        var groupedByPriority = Tasks.GroupBy(t => t.Priority);
        Console.WriteLine("\n4. GroupBy(Priority):");
        foreach (var group in groupedByPriority)
        {
            Console.WriteLine($"   Key: {group.Key} -> {group.Count()} task(s)");
        }

        PromptContinue();
    }

    private static ConsoleColor GetPriorityColor(PriorityLevel priority) => priority switch
    {
        PriorityLevel.Urgent => ConsoleColor.Red,
        PriorityLevel.High => ConsoleColor.DarkYellow,
        PriorityLevel.Medium => ConsoleColor.Cyan,
        PriorityLevel.Low => ConsoleColor.Green,
        _ => ConsoleColor.White
    };

    private static void PromptContinue()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPress any key to return to main menu...");
        Console.ResetColor();
        Console.ReadKey();
    }
}
