namespace HabitTracker.Models;

public class HabitEntry
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public DateTime Date { get; set; }
    public bool IsCompleted { get; set; }

    // Обратная связь
    public Habit? Habit { get; set; }
}