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
        Start start = new(10, 22);

        //Act
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);

        //Assert
        Assert.Equal(2, actual);
    }

    [Fact]
    public void CalculateUniquePlacesCleaned_TenThousandCommands_ShouldTakeLessThanASecond()
    {
        //Arrange

        List<Command> commands = [];
        for (int i = 0; i < 10_000; i++)
        {
            commands.Add(new Command(Direction.South, 1));
        }

        Start start = new(0, 0);

        //Act
        Stopwatch? stopwatch = Stopwatch.StartNew();
        int actual = RobotCalculator.CalculateUniquePlacesCleaned(start, commands);
        stopwatch.Stop();


        //Assert
        Assert.True(stopwatch.Elapsed.TotalSeconds < 1, "calculation should be less than a second");
        testOutputHelper.WriteLine(stopwatch.Elapsed.ToString());
    }
}
