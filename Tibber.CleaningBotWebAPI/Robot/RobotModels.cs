namespace Tibber.CleaningBotWebAPI.Robot;

public record RobotRequest(
    Start Start,
    Command[] Commands
);

public record Start(
    int X,
    int Y
);

public record Command(
    Direction Direction,
    int Steps
);

public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
