using Microsoft.EntityFrameworkCore;
using HabitTracker.Models;

namespace HabitTracker.Data;

public class AppDbContext : DbContext
{
    // Подключение дбконтекста к базе данных через конструктор
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Создание таблицы Habits и HabitEntries
    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<HabitEntry> HabitEntries => Set<HabitEntry>();
}