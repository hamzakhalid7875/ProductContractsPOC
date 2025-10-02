using System;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});
var app = builder.Build();

string? apiUrl = Environment.GetEnvironmentVariable("API_URL");

app.MapGet("/", async () =>
{
    return Results.Content($@"
        <html>
          <body>
            <h1>Frontend running</h1>
            <p>Connected API: {apiUrl}</p>
          </body>
        </html>", "text/html");
});


app.MapGet("/dataView", async () =>
{
    try
    {

        var url =  apiUrl.Split(':');
        var port = url[2];
        var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "http://host.docker.internal:" + port + "/data"); //url for now hardcoded because docker is not allowing outside requests
        var response = await httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        return Results.Content($@"
            <html>
              <body>
                <h1>Data from API</h1>
                <pre>{content}</pre>
              </body>
            </html>", "text/html");
    }
    catch (Exception ex)
    {
        return Results.Content($@"
            <html>
              <body>
                <h1>Error calling API</h1>
                <pre>{ex.Message}</pre>
                <pre>{ex.InnerException?.Message}</pre>
              </body>
            </html>", "text/html");
    }
});



app.Run();
