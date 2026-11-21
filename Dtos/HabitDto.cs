namespace HabitTracker.Dtos;

public record HabitCreateDto(string Name); // DTO для создания привычки, содержит только имя привычки

public record HabitPublicDto(int Id, string Name, DateTime CreatedAt); // отправляем пользователю только то, что нужно, без лишних данных 

public record HabitEntryPublicDto(int Id, int HabitId, DateTime Date, bool IsCompleted); // DTO для отправки пользователю информации о записи привычки, содержит идентификатор записи, идентификатор привычки, дату и статус выполнения

public record DailyStatsDto(int TotalHabits, int CompletedToday, List<HabitPublicDto> Habits);