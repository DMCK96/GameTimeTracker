using System.Windows;
using GameTimeTracker.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Controls;

namespace GameTimeTracker.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    private readonly NotificationService _notificationService;

    public MainWindow()
    {
        InitializeComponent();
        _notificationService = new NotificationService();
        
        // Set up navigation
        NavigationView.SetServiceProvider(CreateServiceProvider());
        NavigationView.Loaded += NavigationView_Loaded;
    }

    private void NavigationView_Loaded(object sender, RoutedEventArgs e)
    {
        // Navigate to Game Library page by default
        NavigationView.Navigate(typeof(Pages.GameLibraryPage));
    }

    private IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        
        // Register services
        services.AddSingleton(_notificationService);
        
        // Register pages
        services.AddSingleton<Pages.GameLibraryPage>();
        services.AddSingleton<Pages.SettingsPage>();
        
        return services.BuildServiceProvider();
    }
}


