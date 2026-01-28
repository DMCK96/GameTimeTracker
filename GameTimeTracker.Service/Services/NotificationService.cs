using Microsoft.Toolkit.Uwp.Notifications;

namespace GameTimeTracker.Service.Services;

public class NotificationService
{
    public void SendStreakNotification(string gameName, int currentStreak)
    {
        new ToastContentBuilder()
            .AddArgument("action", "streak")
            .AddArgument("game", gameName)
            .AddText($"?? {gameName} Streak!")
            .AddText($"You're on a {currentStreak} day streak! Keep it going!")
            .Show();
    }

    public void SendStreakLostNotification(string gameName, int previousStreak)
    {
        new ToastContentBuilder()
            .AddArgument("action", "streakLost")
            .AddArgument("game", gameName)
            .AddText($"?? {gameName} Streak Lost")
            .AddText($"Your {previousStreak} day streak has ended. Start a new one today!")
            .Show();
    }

    public void SendDailyReminderNotification()
    {
        new ToastContentBuilder()
            .AddArgument("action", "reminder")
            .AddText("?? Daily Gaming Reminder")
            .AddText("Don't forget to play today to maintain your streaks!")
            .Show();
    }

    public void SendTestNotification(string title, string message)
    {
        new ToastContentBuilder()
            .AddArgument("action", "test")
            .AddText(title)
            .AddText(message)
            .Show();
    }
}

