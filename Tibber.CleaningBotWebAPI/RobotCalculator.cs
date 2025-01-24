namespace Tibber.CleaningBotWebAPI;

public static class RobotCalculator
{
    public static int CalculateUniquePlacesCleaned(Start start, IEnumerable<Command> commands)
    {
        HashSet<(int, int)> uniquePlacesCleaned = [];
        int x = start.X, y = start.Y;

        // Add the starting position
        uniquePlacesCleaned.Add((x, y));

        // Predefine direction changes for better performance
        Dictionary<Direction, (int dx, int dy)> directionMap = new()
        {
            { Direction.North, (0, 1) },
            { Direction.East, (1, 0) },
            { Direction.South, (0, -1) },
            { Direction.West, (-1, 0) }
        };

        foreach (Command command in commands)
        {
            (int dx, int dy) = directionMap[command.Direction];

            for (int i = 0; i < command.Steps; i++)
            {
                x += dx;
                y += dy;

                uniquePlacesCleaned.Add((x, y));
            }
        }

        return uniquePlacesCleaned.Count;
    }
}
