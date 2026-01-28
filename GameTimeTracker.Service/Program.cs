using GameTimeTracker.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameTimeTracker.Service;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService(options =>
            {
                options.ServiceName = "GameTimeTracker Service";
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<GameMonitoringService>();
                services.AddSingleton<NotificationService>();
            });
}
