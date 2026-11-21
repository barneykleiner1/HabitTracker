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
    public async Task<HabitEntry> CreateEntryAsync(HabitEntry entry) 
    {
        await _db.HabitEntries.AddAsync(entry); // добавляем новую запись привычки в базу данных
        await _db.SaveChangesAsync(); // сохраняем изменения в базе данных
        return entry; // возвращаем созданную запись привычки
    }
    public async Task<HabitEntry?> GetEntryByDateAsync(int habitId, DateTime date) 
    {
        return await _db.HabitEntries.FirstOrDefaultAsync(e => e.HabitId == habitId && e.Date == date.Date); 
    }
    public async Task<Habit?> GetByIdAsync(int id)
    {
        return await _db.Habits.FirstOrDefaultAsync(h => h.Id == id); // возвращаем привычку по идентификатору
    }
    public async Task DeleteAsync(Habit habit) 
    {
        _db.Habits.Remove(habit);
        await _db.SaveChangesAsync(); //  удаляем привычку и сохраняем изменения в базе данных
    }
    public async Task<List<HabitEntry>> GetEntriesByDateAsync(DateTime date)
    {
        return await _db.HabitEntries.Where(e => e.Date.Date == date.Date).ToListAsync(); // возвращаем все записи привычек за указанную дату(у нас за сегодняшний день, хотя по факту дата может быть любой)
    }
    public async Task DeleteEntryAsync(HabitEntry entry)
    {
        _db.HabitEntries.Remove(entry);
        await _db.SaveChangesAsync();
    }
    public async Task<HabitEntry?> GetEntryAsync(int entryId)
    {
        return await _db.HabitEntries.FirstOrDefaultAsync(e => e.Id == entryId);
    }
    public async Task<List<HabitEntry>> GetEntriesByHabitIdAsync(int habitId)
    {
        return await _db.HabitEntries.Where(e => e.HabitId == habitId).ToListAsync();
    }
}