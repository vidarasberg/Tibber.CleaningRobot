namespace Tibber.CleaningBotWebAPI.Robot;

public static class RobotCalculator
{
    private record struct Line(int Start, int End, int CrossAxisCoordinate);

    public static int CalculateUniquePlacesCleaned(Start start, IEnumerable<Command> commands)
    {
        var horizontalLines = new List<Line>();
        var verticalLines = new List<Line>();

        int x = start.X, y = start.Y;
        horizontalLines.Add(new Line(x, x, y));
        verticalLines.Add(new Line(y, y, x));

        foreach (var command in commands)
        {
            switch (command.Direction)
            {
                case Direction.East:
                    horizontalLines.Add(new Line(x, x + command.Steps, y));
                    x += command.Steps;
                    break;
                case Direction.West:
                    horizontalLines.Add(new Line(x - command.Steps, x, y));
                    x -= command.Steps;
                    break;
                case Direction.North:
                    verticalLines.Add(new Line(y, y + command.Steps, x));
                    y += command.Steps;
                    break;
                case Direction.South:
                    verticalLines.Add(new Line(y - command.Steps, y, x));
                    y -= command.Steps;
                    break;
            }
        }

        return CountUniquePoints(horizontalLines, verticalLines);
    }

    private static int CountUniquePoints(List<Line> horizontalLines, List<Line> verticalLines)
    {
        var cleanedPoints = CountPointsInLines(horizontalLines);
        cleanedPoints += CountPointsInLines(verticalLines);

        var intersections = CountIntersections(horizontalLines, verticalLines);

        return cleanedPoints - intersections;
    }

    private static int CountPointsInLines(List<Line> lines)
    {
        return lines
            .GroupBy(line => line.CrossAxisCoordinate)
            .SelectMany(MergeLines)
            .Sum(line => line.End - line.Start + 1);
    }

    private static List<Line> MergeLines(IEnumerable<Line> lines)
    {
        var result = new List<Line>();
        Line? current = null;

        foreach (var line in lines.OrderBy(s => s.Start))
        {
            if (current == null)
            {
                current = line;
                continue;
            }

            if (current.Value.End >= line.Start - 1)
                current = current.Value with { End = Math.Max(current.Value.End, line.End) };
            else
            {
                result.Add(current.Value);
                current = line;
            }
        }

        if (current.HasValue)
            result.Add(current.Value);

        return result;
    }

    private static int CountIntersections(List<Line> horizontalLines, List<Line> verticalLines)
    {
        return horizontalLines
            .SelectMany(h => verticalLines.Select(v => new { Horizontal = h, Vertical = v }))
            .Where(pair => LinesIntersect(pair.Horizontal, pair.Vertical))
            .Select(pair => new { X = pair.Vertical.CrossAxisCoordinate, Y = pair.Horizontal.CrossAxisCoordinate })
            .Distinct()
            .Count();
    }

    private static bool LinesIntersect(Line horizontal, Line vertical)
    {
        bool verticalCrossesHorizontal = vertical.Start <= horizontal.CrossAxisCoordinate
                                     && vertical.End >= horizontal.CrossAxisCoordinate;

        bool horizontalCrossesVertical = horizontal.Start <= vertical.CrossAxisCoordinate
                                     && horizontal.End >= vertical.CrossAxisCoordinate;

        return verticalCrossesHorizontal && horizontalCrossesVertical;
    }
}
