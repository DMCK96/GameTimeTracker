using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameTimeTracker.Service.Services;

public class GameMonitoringService : BackgroundService
{
    private readonly ILogger<GameMonitoringService> _logger;
    private readonly NotificationService _notificationService;

    public GameMonitoringService(
        ILogger<GameMonitoringService> logger,
        NotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("GameMonitoringService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: Implement game process monitoring logic
            // For now, just log that we're running
            _logger.LogDebug("Service is monitoring at: {time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _logger.LogInformation("GameMonitoringService is stopping.");
    }
}
