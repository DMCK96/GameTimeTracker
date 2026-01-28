using System.Windows;
using GameTimeTracker.UI.Services;
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
    }

    private void TestStreakNotification_Click(object sender, RoutedEventArgs e)
    {
        _notificationService.SendStreakNotification("Elden Ring", 7);
    }

    private void TestReminderNotification_Click(object sender, RoutedEventArgs e)
    {
        _notificationService.SendDailyReminderNotification();
    }

    private void TestCustomNotification_Click(object sender, RoutedEventArgs e)
    {
        _notificationService.SendTestNotification(
            "🎮 GameTimeTracker",
            "This is a test notification! Everything is working correctly.");
    }
}
