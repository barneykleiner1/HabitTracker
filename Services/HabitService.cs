using HabitTracker.Dtos;
using HabitTracker.Models;
using HabitTracker.Repositories;

public class HabitAlreadyExistsException : Exception
{
    public HabitAlreadyExistsException(string name) : base($"Привычка с именем {name} уже существует") 
    {
    
    }
}
