var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://+:5000");

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/tibber-developer-test/enter-path", () =>
    {
        throw new NotImplementedException();
    })
    .WithName("EnterPath");

app.Run();
