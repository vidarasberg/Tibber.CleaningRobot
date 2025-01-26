using Tibber.CleaningBotWebAPI.Utils;

namespace UnitTests;

public class StopwatchUtilityTests
{
    private static int SomeCalculation()
    {
        Thread.Sleep(10);
        return 42;
    }

    [Fact]
    public void MeasureExecutionTime_ExecutingFunc_ShouldIncreaseElapsed()
    {
        //Act
        int result = StopwatchUtility.MeasureExecutionTime(
            SomeCalculation,
            out TimeSpan elapsed);

        //Assert
        Assert.Equal(42, result);
        Assert.True(elapsed > TimeSpan.Zero, "Some time should have been counted");
    }
}
