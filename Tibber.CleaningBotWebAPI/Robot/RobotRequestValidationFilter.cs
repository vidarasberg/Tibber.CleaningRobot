using Microsoft.AspNetCore.Mvc;

namespace Tibber.CleaningBotWebAPI.Robot;

public class RobotRequestValidationFilter : IEndpointFilter
{
    private readonly ILogger _logger;

    public RobotRequestValidationFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RobotRequestValidationFilter>();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext efiContext,
        EndpointFilterDelegate next)
    {
        RobotRequest body = efiContext.GetArgument<RobotRequest>(0);

        (bool isValid, string errors) = Validator.IsValid(body);

        if (isValid)
        {
            return await next(efiContext);
        }

        _logger.LogWarning("Validation error {ValidationErrors}", errors);
        ProblemDetails problem = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Detail = errors
        };
        return Results.Problem(problem);
    }
}

public static class Validator
{
    public static (bool, string) IsValid(RobotRequest req)
    {
        List<string> validationResults = [];
        if (req.Start.X is > 100_000 or < -100_000)
        {
            validationResults.Add("Start X needs to be between -100 000 and 100 000.");
        }

        if (req.Start.Y is > 100_000 or < -100_000)
        {
            validationResults.Add("Start Y needs to be between -100 000 and 100 000.");
        }

        if (req.Commands.Length > 10_000)
        {
            validationResults.Add("Max number of commands is 10 000");
        }

        bool isValid = validationResults.Count == 0;
        string errors = string.Join(',', validationResults);

        return (isValid, errors);
    }
}
