namespace GameTimeTracker.Data.Models;

public class UserSettings
{
    public int Id { get; set; }
    public TimeSpan MinimumDailyPlaytime { get; set; } = TimeSpan.FromMinutes(30);
    public bool NotificationsEnabled { get; set; } = true;
    public TimeSpan? DailyReminderTime { get; set; }
    public string Theme { get; set; } = "System";
    public string? AccentColor { get; set; }
}
