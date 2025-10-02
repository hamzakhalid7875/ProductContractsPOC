var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});
var app = builder.Build();

app.MapGet("/auth", () =>
{
    return Results.Ok(new { message = "Auth service running", token = Guid.NewGuid() });
});

app.Run();
