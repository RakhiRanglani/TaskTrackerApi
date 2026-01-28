using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaskTrackerApi.BackgroundServices;

public class TaskMonitorService : BackgroundService
{
    private readonly ILogger<TaskMonitorService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public TaskMonitorService(
        ILogger<TaskMonitorService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TaskMonitorService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var taskService = scope.ServiceProvider.GetRequiredService<Services.ITaskService>();

            var tasks = await taskService.GetAllAsync();
            var incompleteCount = tasks.Count(t => !t.IsCompleted);

            _logger.LogInformation(
                "Background check: {Count} incomplete tasks",
                incompleteCount);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
