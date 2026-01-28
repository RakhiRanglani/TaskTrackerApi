using TaskTrackerApi.BackgroundServices;
using TaskTrackerApi.Dtos;
using TaskTrackerApi.Services;
using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register services FIRST
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddHostedService<TaskMonitorService>();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasktracker.db"));

// ❌ Nothing should be added after this line
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/tasks", async (ITaskService taskService) =>
{
    return await taskService.GetAllAsync();
});

app.MapPost("/tasks", async(CreateTaskRequest request, ITaskService taskService) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest("Task title is required");
    }

    var task = await taskService.AddAsync(request.Title);

    if (task is null)
    {
        return Results.Conflict("A task with the same title already exists");
    }

    return Results.Created($"/tasks/{task.Id}", task);
});
app.MapDelete("/tasks/{id:int}", async (int id, ITaskService taskService) =>
{
    var deleted = await taskService.DeleteAsync(id);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.MapPut("/tasks/{id:int}", async (
    int id,
    UpdateTaskRequest request,
    ITaskService taskService) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest("Task title is required");
    }

    var updated = await taskService.UpdateAsync(id, request.Title, request.IsCompleted);

    if (updated is null)
    {
        return Results.Conflict("Task not found or duplicate title");
    }

    return Results.Ok(updated);
});

app.Run();
