namespace GameTimeTracker.Data.Models;

public class Game
{
    public int Id { get; set; }
    public required string ProcessName { get; set; }
    public required string DisplayName { get; set; }
    public string? IconPath { get; set; }
    public TimeSpan TotalPlaytime { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public DateTime? LastPlayedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<PlaySession> PlaySessions { get; set; } = new List<PlaySession>();
}
