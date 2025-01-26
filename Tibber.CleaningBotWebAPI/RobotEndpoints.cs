using System.Diagnostics;

namespace Tibber.CleaningBotWebAPI;

public static class RobotEndpoints
{
    public static void MapRobotEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/tibber-developer-test");
        group.MapPost("/enter-path", async (RobotRequest body, RobotDbContext robotDbContext) =>
            {
                Stopwatch stopWatch = new();

                stopWatch.Start();
                int uniquePlacesCleaned = RobotCalculator.CalculateUniquePlacesCleaned(body.Start, body.Commands);
                stopWatch.Stop();

                ExecutionRecord execution = new()
                {
                    Commands = body.Commands.Length,
                    Duration = stopWatch.Elapsed.TotalSeconds,
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
