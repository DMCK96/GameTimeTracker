namespace GameTimeTracker.Data.Models;

public class PlaySession
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public Game Game { get; set; } = null!;
}
