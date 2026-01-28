using System.Windows;
using System.Windows.Controls;
using GameTimeTracker.UI.Services;

namespace GameTimeTracker.UI.Pages;

public partial class SettingsPage : Page
{
    private readonly NotificationService _notificationService;

    public SettingsPage(NotificationService notificationService)
    {
        InitializeComponent();
        _notificationService = notificationService;
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
            "?? GameTimeTracker",
            "This is a test notification! Everything is working correctly.");
    }
}

