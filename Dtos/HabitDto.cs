namespace HabitTracker.Dtos;

public record HabitCreateDto(string Name); // DTO для создания привычки, содержит только имя привычки

public record HabitPublicDto(int Id, string Name, DateTime CreatedAt); // отправляем пользователю только то, что нужно, без лишних данных 