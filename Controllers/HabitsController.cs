using Microsoft.AspNetCore.Mvc;
using HabitTracker.Dtos;
using HabitTracker.Services;

namespace HabitTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HabitsController : ControllerBase
{
    private readonly IHabitService _service;

    // Теперь контроллер зависит ТОЛЬКО от Сервиса!
    public HabitsController(IHabitService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<HabitPublicDto>>> GetAll()
    {
        var habits = await _service.GetAllAsync();
        return Ok(habits);
    }

    [HttpPost]
    public async Task<ActionResult<HabitPublicDto>> Create(HabitCreateDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
        catch (HabitAlreadyExistsException ex)
        {
            // Ловим бизнес-ошибку и превращаем её в HTTP 409 Conflict
            return Conflict(ex.Message);
        }
    }
    [HttpPost("{id}/entries")]
    public async Task<ActionResult<HabitEntryPublicDto>> CreateEntry(int id)
    {
        try
        {
            var result = await _service.CreateEntryAsync(id);
            return Ok(result);
        }
        catch (HabitEntryAlreadyExistsException ex) // Сначала специфичное
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex) // Потом всё остальное!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            return NotFound(ex.Message);
        }
    }
    [HttpDelete("{habitId}/entries/{entryId}")]
    public async Task<IActionResult> DeleteEntry(int habitId, int entryId)
    {
        try
        {
            await _service.DeleteEntryAsync(habitId, entryId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) // Если отметка не найдена
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex) // Если отметка чужая
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("today")]
    public async Task<ActionResult<DailyStatsDto>> GetToday()
    {
        var stats = await _service.GetTodayStatsAsync();
        return Ok(stats);
    }
    [HttpGet("{id}/entries")]
    public async Task<ActionResult<List<HabitEntryPublicDto>>> GetEntries(int id)
    {
        try
        {
            var result = await _service.GetEntriesByHabitIdAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}