var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});
var app = builder.Build();

string? authUrl = Environment.GetEnvironmentVariable("AUTH_URL");

app.MapGet("/api", () =>
{
    return Results.Ok(new { message = "API service running", auth = authUrl });
});

app.MapGet("/data", () =>
{
    return Results.Ok("This data is sent from API Product");
});

app.Run();
