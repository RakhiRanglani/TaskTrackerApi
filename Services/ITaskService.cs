using TaskTrackerApi.Models;

namespace TaskTrackerApi.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> AddAsync(string title);
    Task<TaskItem?> UpdateAsync(int id, string title, bool isCompleted);
    Task<bool> DeleteAsync(int id);
}
