using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Data;
using TaskTrackerApi.Models;

namespace TaskTrackerApi.Services;

public class TaskService : ITaskService
{
    private readonly TaskDbContext _db;

public TaskService(TaskDbContext db)
{
    _db = db;
}

public async Task<IEnumerable<TaskItem>> GetAllAsync()
{
    return await _db.Tasks.ToListAsync();
}

public async Task<TaskItem?> AddAsync(string title)
{
     var task = new TaskItem
    {
        Title = title,
        IsCompleted = false
    };

    _db.Tasks.Add(task);

    try
    {
        await _db.SaveChangesAsync();
        return task;
    }
    catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
    {
        return null;
    }
}


public async Task<TaskItem?> UpdateAsync(int id, string title, bool isCompleted)
{
   var existing = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    if (existing is null)
        return null;

    existing.Title = title;
    existing.IsCompleted = isCompleted;

    try
    {
        await _db.SaveChangesAsync();
        return existing;
    }
    catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
    {
        return null;
    }
}


public async Task<bool> DeleteAsync(int id)
{
    var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    if (task is null)
    {
        return await Task.FromResult(false);
    }

    _db.Tasks.Remove(task);
    return await Task.FromResult(true);
}
private static bool IsUniqueConstraintViolation(DbUpdateException ex)
{
    return ex.InnerException?.Message.Contains(
        "UNIQUE constraint failed",
        StringComparison.OrdinalIgnoreCase) == true;
}
}