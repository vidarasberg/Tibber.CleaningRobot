using System.ComponentModel.DataAnnotations;

namespace Tibber.CleaningBotWebAPI.Robot;

public class ExecutionRecord
{
    [Key] public int Id { get; private set; }
    public DateTime Timestamp { get; init; }
    public int Commands { get; init; }
    public int Result { get; init; }
    public double Duration { get; init; }
}
