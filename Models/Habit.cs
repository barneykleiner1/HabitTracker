namespace HabitTracker.Models;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Связь один-ко-многим
    public List<HabitEntry> Entries { get; set; } = new();
}