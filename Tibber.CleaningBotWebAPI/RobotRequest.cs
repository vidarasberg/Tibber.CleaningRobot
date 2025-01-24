namespace Tibber.CleaningBotWebAPI;

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
