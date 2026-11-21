using HabitTracker.Dtos;
using HabitTracker.Models;
using HabitTracker.Repositories;
namespace HabitTracker.Services;

public class HabitAlreadyExistsException : Exception
{
    public HabitAlreadyExistsException(string name) : base($"Привычка с именем {name} уже существует") 
    {
    
    }
}
public class HabitEntryAlreadyExistsException : Exception
{
    public HabitEntryAlreadyExistsException() : base($"Запись привычки на сегодня уже существует")
    {
    
    }
}
public interface IHabitService 
{
    Task<List<HabitPublicDto>> GetAllAsync();
    Task<HabitPublicDto> CreateAsync(HabitCreateDto dto);
    Task<HabitEntryPublicDto> CreateEntryAsync(int habitId);
    Task DeleteAsync(int id);
    Task<DailyStatsDto> GetTodayStatsAsync();
    Task DeleteEntryAsync(int habitId, int entryId);
    Task<List<HabitEntryPublicDto>> GetEntriesByHabitIdAsync(int habitId);
}
public class HabitService : IHabitService 
{
    private readonly IHabitRepository _repository;

    public HabitService(IHabitRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<HabitPublicDto>> GetAllAsync()
    {
        var habits = await _repository.GetAllAsync();
        return habits.Select(h => new HabitPublicDto(h.Id, h.Name, h.CreatedAt)).ToList();
    }
    public async Task<HabitPublicDto> CreateAsync(HabitCreateDto dto) 
    {
        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing != null)
        {
            throw new HabitAlreadyExistsException(dto.Name);
        }
        var habit = new Habit { Name = dto.Name };
        await _repository.CreateAsync(habit);
        return new HabitPublicDto(habit.Id, habit.Name, habit.CreatedAt);
    }
    public async Task<HabitEntryPublicDto> CreateEntryAsync(int habitId)
    {
        if (await _repository.GetByIdAsync(habitId) == null)
        {
            throw new Exception($"Привычка с идентификатором {habitId} не найдена");
        }
        var existingEntry = await _repository.GetEntryByDateAsync(habitId, DateTime.UtcNow);
        if (existingEntry != null)
        {
            throw new HabitEntryAlreadyExistsException();
        }
        var HabitEntry = new HabitEntry { HabitId = habitId, Date = DateTime.UtcNow, IsCompleted = true };
        await _repository.CreateEntryAsync(HabitEntry);
        return new HabitEntryPublicDto(HabitEntry.Id, HabitEntry.HabitId, HabitEntry.Date, HabitEntry.IsCompleted);
    }
    public async Task DeleteAsync(int id)
    {
        var habit = await _repository.GetByIdAsync(id);
        if (habit == null)
        {
            throw new Exception($"Привычка с идентификатором {id} не найдена");
        }
        await _repository.DeleteAsync(habit);
    }
    public async Task<DailyStatsDto> GetTodayStatsAsync() 
    {
        var habits = await _repository.GetAllAsync();
        var entries = await _repository.GetEntriesByDateAsync(DateTime.UtcNow);
        var habitIds = habits.Select(h => h.Id).ToList();
        var completedToday = entries
            .Where(e => e.IsCompleted && habitIds.Contains(e.HabitId))
            .Select(e => e.HabitId)
            .Distinct()
            .Count();
        var habitDtos = habits.Select(h => new HabitPublicDto(h.Id, h.Name, h.CreatedAt)).ToList();
        return new DailyStatsDto(habits.Count, completedToday, habitDtos);
    }
    public async Task DeleteEntryAsync(int habitId, int entryId)
    {
        var entry = await _repository.GetEntryAsync(entryId);
        if (entry == null) 
        {
            throw new InvalidOperationException($"Запись привычки с идентификатором {entryId} не найдена");
        }
        if (entry.HabitId != habitId) // <-- Сравниваем ID привычки из базы с ID привычки из URL (слава дипсику, у меня здесь был баг)
        {
            throw new InvalidOperationException($"Запись привычки с идентификатором {entryId} не принадлежит привычке с идентификатором {habitId}");
        }
        await _repository.DeleteEntryAsync(entry);
    }
    public async Task<List<HabitEntryPublicDto>> GetEntriesByHabitIdAsync(int habitId)
    {
        var habit = await _repository.GetByIdAsync(habitId);
        if (habit == null) throw new KeyNotFoundException($"Привычка с id={habitId} не найдена");

        var entries = await _repository.GetEntriesByHabitIdAsync(habitId);
        return entries.Select(e => new HabitEntryPublicDto(e.Id, e.HabitId, e.Date, e.IsCompleted)).ToList();
    }
}