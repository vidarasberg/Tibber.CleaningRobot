using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

using Tibber.CleaningBotWebAPI.Robot;

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

if (app.Environment.IsDevelopment())
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        RobotDbContext db = scope.ServiceProvider.GetRequiredService<RobotDbContext>();
        await db.Database.MigrateAsync();
    }
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapRobotEndpoints();

app.Run();
