using System.Diagnostics;

namespace Tibber.CleaningBotWebAPI.Utils;

public static class StopwatchUtility
{
    public static T MeasureExecutionTime<T>(Func<T> func, out TimeSpan elapsed)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        T result = func();
        stopwatch.Stop();
        elapsed = stopwatch.Elapsed;

        return result;
    }
}
