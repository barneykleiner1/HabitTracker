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
}