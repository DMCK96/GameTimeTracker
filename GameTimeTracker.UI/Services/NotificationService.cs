using GameTimeTracker.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wpf.Ui.Controls;

namespace GameTimeTracker.UI.Services;

public class NotificationService
{
    private readonly List<ToastNotificationWindow> _activeNotifications = new();
    private readonly SettingsService _settingsService;
    private const int NotificationSpacing = 10;

    public NotificationService(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public void SendTestNotification(string title, string message)
    {
        ShowNotification(title, message, SymbolRegular.Info24);
    }

    public void SendStreakNotification(string gameName, int currentStreak)
    {
        ShowNotification(
            $"?? {gameName} Streak!",
            $"You're on a {currentStreak} day streak! Keep it going!",
            SymbolRegular.Trophy24);
    }

    public void SendStreakLostNotification(string gameName, int previousStreak)
    {
        ShowNotification(
            $"?? {gameName} Streak Lost",
            $"Your {previousStreak} day streak has ended. Start a new one today!",
            SymbolRegular.HeartBroken24);
    }

    public void SendDailyReminderNotification()
    {
        ShowNotification(
            "?? Daily Gaming Reminder",
            "Don't forget to play today to maintain your streaks!",
            SymbolRegular.Alert24);
    }

    private void ShowNotification(string title, string message, SymbolRegular icon, int autoCloseDurationSeconds = 5)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var settings = _settingsService.GetSettings();
            
            if (!settings.NotificationsEnabled)
                return;
            
            var duration = settings.AutoDismissNotifications ? settings.AutoDismissDurationSeconds : 0;
            var notification = new ToastNotificationWindow(
                title, 
                message, 
                icon, 
                duration,
                settings.NotificationLocation,
                settings.DismissHotkey);
            
            // Position notification considering other active notifications
            PositionNotification(notification);
            
            // Track the notification
            _activeNotifications.Add(notification);
            
            // Remove from tracking when closed
            notification.Closed += (s, e) =>
            {
                _activeNotifications.Remove(notification);
                RepositionNotifications();
            };
            
            notification.Show();
        });
    }

    private void PositionNotification(ToastNotificationWindow notification)
    {
        var workArea = SystemParameters.WorkArea;
        var yOffset = workArea.Bottom - notification.Height - 20;
        
        // Stack notifications from bottom to top
        foreach (var existing in _activeNotifications.AsEnumerable().Reverse())
        {
            yOffset -= (existing.Height + NotificationSpacing);
        }
        
        notification.Top = yOffset;
    }

    private void RepositionNotifications()
    {
        var workArea = SystemParameters.WorkArea;
        var yOffset = workArea.Bottom - 20;
        
        foreach (var notification in _activeNotifications.AsEnumerable().Reverse())
        {
            yOffset -= notification.Height;
            notification.Top = yOffset;
            yOffset -= NotificationSpacing;
        }
    }
}

