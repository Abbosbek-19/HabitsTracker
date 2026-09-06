namespace DailyTaskTracker.Domain.Entities;

/// <summary>
/// User-defined or system default category for grouping tasks.
/// </summary>
public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string ColorHex { get; private set; } = "#3B82F6";
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public List<TaskItem> Tasks { get; private set; } = new();

    private Category() { }

    public Category(string name, string colorHex, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        Name = name.Trim();
        ColorHex = string.IsNullOrWhiteSpace(colorHex) ? "#3B82F6" : colorHex.Trim();
        UserId = userId;
    }
}
