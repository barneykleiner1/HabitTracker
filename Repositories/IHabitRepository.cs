using HabitTracker.Models;

namespace HabitTracker.Repositories;

public interface IHabitRepository
{
    Task<List<Habit>> GetAllAsync();
    Task<Habit?> GetByNameAsync(string name);
    Task<Habit> CreateAsync(Habit habit);
    Task<HabitEntry> CreateEntryAsync(HabitEntry entry);
    Task<HabitEntry?> GetEntryByDateAsync(int habitId, DateTime date);
    Task<Habit?> GetByIdAsync(int id);
    Task DeleteAsync(Habit habit);
    Task<List<HabitEntry>> GetEntriesByDateAsync(DateTime date);
    Task DeleteEntryAsync(HabitEntry entry);
    Task<HabitEntry?> GetEntryAsync(int entryId);
    Task<List<HabitEntry>> GetEntriesByHabitIdAsync(int habitId);
}