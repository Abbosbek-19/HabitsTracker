using DailyTaskTracker.Application.DTOs.Habits;

namespace DailyTaskTracker.Application.Interfaces;

public interface IHabitService
{
    Task<IEnumerable<HabitResponse>> GetAllHabitsAsync(Guid userId);
    Task<HabitResponse?> GetHabitByIdAsync(Guid id, Guid userId);
    Task<HabitResponse> CreateHabitAsync(CreateHabitRequest request, Guid userId);
    Task<HabitResponse?> UpdateHabitAsync(Guid id, UpdateHabitRequest request, Guid userId);
    Task<bool> DeleteHabitAsync(Guid id, Guid userId);
    Task<HabitResponse?> RecordCompletionAsync(Guid habitId, DateOnly? date, Guid userId);
}
