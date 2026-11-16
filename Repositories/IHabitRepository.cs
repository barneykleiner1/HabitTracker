using HabitTracker.Models;

namespace HabitTracker.Repositories;

public interface IHabitRepository
{
    Task<List<Habit>> GetAllAsync();
    Task<Habit?> GetByNameAsync(string name);
    Task<Habit> CreateAsync(Habit habit);
}