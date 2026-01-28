namespace GameTimeTracker.Data.Models;

public class UserSettings
{
    public int Id { get; set; }
    public TimeSpan MinimumDailyPlaytime { get; set; } = TimeSpan.FromMinutes(30);
    public bool NotificationsEnabled { get; set; } = true;
    public TimeSpan? DailyReminderTime { get; set; }
    public string Theme { get; set; } = "System";
    public string? AccentColor { get; set; }
    
    // Notification Settings
    public NotificationLocation NotificationLocation { get; set; } = NotificationLocation.BottomRight;
    public bool AutoDismissNotifications { get; set; } = true;
    public int AutoDismissDurationSeconds { get; set; } = 5;
    public string DismissHotkey { get; set; } = "Escape";
    public List<TimeSpan> StreakReminderTimes { get; set; } = new();
    public TimeSpan StreakMinimumGameTime { get; set; } = TimeSpan.FromMinutes(30);
}

public enum NotificationLocation
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}
