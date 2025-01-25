using System.Diagnostics;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using Tibber.CleaningBotWebAPI;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://+:5000");

builder.Services.AddDbContext<RobotDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreDB")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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

app.Run();
