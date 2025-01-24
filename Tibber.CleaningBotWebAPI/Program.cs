using System.Diagnostics;
using System.Text.Json.Serialization;

using Tibber.CleaningBotWebAPI;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://+:5000");

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
group.MapPost("/enter-path", (RobotRequest body) =>
    {
        Stopwatch stopWatch = new();

        stopWatch.Start();
        RobotCalculator.CalculateUniquePlacesCleaned(body.Start, body.Commands);
        stopWatch.Stop();
        return Results.Ok(stopWatch.Elapsed.Seconds);
    })
    .WithName("EnterPath")
    .AddEndpointFilter<RobotRequestValidationFilter>()
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.Run();


public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
