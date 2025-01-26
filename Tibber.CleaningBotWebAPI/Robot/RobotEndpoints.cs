using Tibber.CleaningBotWebAPI.Utils;

namespace Tibber.CleaningBotWebAPI.Robot;

public static class RobotEndpoints
{
    public static void MapRobotEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/tibber-developer-test");
        group.MapPost("/enter-path", async (RobotRequest body, RobotDbContext robotDbContext) =>
            {
                int uniquePlacesCleaned = StopwatchUtility.MeasureExecutionTime(
                    () => RobotCalculator.CalculateUniquePlacesCleaned(body.Start, body.Commands),
                    out TimeSpan elapsed);

                ExecutionRecord execution = new()
                {
                    Commands = body.Commands.Length,
                    Duration = elapsed.TotalSeconds,
                    Result = uniquePlacesCleaned,
                    Timestamp = DateTime.UtcNow
                };

                await robotDbContext.AddAsync(execution);
                await robotDbContext.SaveChangesAsync();

                ExecutionRecord? savedExecution = await robotDbContext.Executions.FindAsync(execution.Id);

                return Results.Ok(savedExecution);
            })
            .WithName("EnterPath")
            .AddEndpointFilter<RobotRequestValidationFilter>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
