using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Models;

namespace HabitTracker.Repositories;

public class HabitRepository : IHabitRepository 
{
    private readonly AppDbContext _db;

    public HabitRepository(AppDbContext db)
    {
        _db = db;
    } // это конструктор, который принимает AppDbContext и сохраняет его в приватное поле _db
    public async Task<List<Habit>> GetAllAsync() 
    {
        return await _db.Habits.ToListAsync(); // возвращаем все привычки из базы данных
    }
    public async Task<Habit?> GetByNameAsync(string name) 
    {
        return await _db.Habits.FirstOrDefaultAsync(h => h.Name == name); // возвращаем привычку по имени, если она существует
    }
    public async Task<Habit> CreateAsync(Habit habit) 
    {
        await _db.Habits.AddAsync(habit); // добавляем новую привычку в базу данных
        await _db.SaveChangesAsync(); // сохраняем изменения в базе данных
        return habit; // возвращаем созданную привычку
    }
}