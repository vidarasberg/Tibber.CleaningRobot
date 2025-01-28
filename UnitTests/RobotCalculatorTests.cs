using System.Diagnostics;

using Tibber.CleaningBotWebAPI.Robot;

using Xunit.Abstractions;

namespace UnitTests;

public class RobotCalculatorTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void CalculateUniquePlacesCleaned_NoCommands_ReturnsOne()
    {
        //Arrange
        Command[] commands = [];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(1, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_GoSouthOneStepCommands_ReturnsTwo()
    {
        //Arrange
        Command[] commands = [new(Direction.South, 1)];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(2, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_GoSouthTwoStepsCommands_ReturnsThree()
    {
        //Arrange
        Command[] commands = [new(Direction.South, 2)];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(3, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_MultipleCommands_ReturnsExpected()
    {
        //Arrange
        Command[] commands = [new(Direction.East, 2), new(Direction.North, 1)];
        Start start = new(10, 22);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(4, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_GoingBackAndForth_OnlyCountsEachPlaceOnce()
    {
        //Arrange
        Command[] commands =
        [
            new(Direction.South, 1),
            new(Direction.North, 1),
            new(Direction.South, 1),
            new(Direction.North, 1)
        ];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(2, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_CrossingVerticalAndHorizontalLinesSouthFirst_OnlyCountsEachPlaceOnce()
    {
        //Arrange

        //   start
        //    ↓
        //   _|_
        //    |_|_
        //      |_|
        Command[] commands =
        [
            new(Direction.South, 2),
            new(Direction.East, 2),
            new(Direction.South, 1),
            new(Direction.West, 1),
            new(Direction.North, 2),
            new(Direction.West, 2),
        ];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(9, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_CrossingVerticalAndHorizontalLinesNorthFirst_OnlyCountsEachPlaceOnce()
    {
        //Arrange

        //     _
        //   _|_|
        //    |
        //    ↑ 
        // start
        Command[] commands =
        [
            new(Direction.North, 2),
            new(Direction.East, 1),
            new(Direction.South, 1),
            new(Direction.West, 2),
        ];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(6, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_CrossingVerticalAndHorizontalLinesEastFirst_OnlyCountsEachPlaceOnce()
    {
        //Arrange

        // start →  _|_
        //           |_|

        Command[] commands =
        [
            new(Direction.East, 2),
            new(Direction.South, 1),
            new(Direction.West, 1),
            new(Direction.North, 2),
        ];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(6, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_CrossingVerticalAndHorizontalLinesWestFirst_OnlyCountsEachPlaceOnce()
    {
        //Arrange

        //    _|_ ← start
        //   |_|
        Command[] commands =
        [
            new(Direction.West, 2),
            new(Direction.South, 1),
            new(Direction.East, 1),
            new(Direction.North, 2),
        ];
        Start start = new(0, 0);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(6, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_TenThousandCommands_ShouldTakeLessThanATenSeconds()
    {
        // Arrange
        var random = new Random(42); // Fixed seed for reproducibility
        var directions = Enum.GetValues<Direction>();
        List<Command> commands = [];

        for (int i = 0; i < 10_000; i++)
        {
            Direction direction = directions[random.Next(directions.Length)];
            commands.Add(new Command(direction, 99_999));
        }
        Start start = new(0, 0);

        // Act
        Stopwatch stopwatch = Stopwatch.StartNew();
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);
        stopwatch.Stop();

        // Assert
        Assert.Equal(401694868, actual);
        testOutputHelper.WriteLine($"Unique places cleaned: {actual}");
        Assert.True(stopwatch.Elapsed.TotalSeconds < 10, "calculation should be less than 10 seconds");
        Assert.True(Process.GetCurrentProcess().WorkingSet64 < 100_000_000, "memory usage should be less than 100MB");
        testOutputHelper.WriteLine($"Time: {stopwatch.Elapsed}, Memory: {Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024}MB");
    }
}
