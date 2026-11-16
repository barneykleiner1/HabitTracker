using System.Reflection.Metadata.Ecma335; // ???
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
public interface IHabitService 
{
    Task<List<HabitPublicDto>> GetAllAsync();
    Task<HabitPublicDto> CreateAsync(HabitCreateDto dto);
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
}