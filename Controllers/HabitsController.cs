using Microsoft.AspNetCore.Mvc;
using HabitTracker.Data;
using HabitTracker.Dtos;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Models;

namespace HabitTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HabitsController : ControllerBase 
{
    private readonly AppDbContext _db;
    public HabitsController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<List<HabitPublicDto>>> GetAll() 
    {
    var habits = await _db.Habits
        .Select(h => new HabitPublicDto(h.Id, h.Name, h.CreatedAt))
        .ToListAsync();
    return Ok(habits);
    }
    [HttpPost]
    public async Task<ActionResult<HabitPublicDto>> Create(HabitCreateDto habitDto)
    {
        var habit = new Habit { Name = habitDto.Name };
        _db.Habits.Add(habit);
        await _db.SaveChangesAsync();
        var newDto = new HabitPublicDto(habit.Id, habit.Name, habit.CreatedAt);
        return Ok(newDto);
    }
}
