var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

LoadPropertiesIntoEnvironment("/config/application.properties", "application.properties.from.configmap");
LoadPropertiesIntoEnvironment("/secret-config/application.secret.properties", "application.secret.properties.from.secret");

app.MapGet("/api/hello", () => "Hello, EDP!");

app.MapGet("/env", () =>
{
    var envVars = Environment.GetEnvironmentVariables()
        .Cast<System.Collections.DictionaryEntry>()
        .ToDictionary(entry => (string)entry.Key, entry => (string?)entry.Value);

    return Results.Ok(envVars);
});

app.Run();

static void LoadPropertiesIntoEnvironment(string filePath, string environmentVariableName)
{
    if (File.Exists(filePath))
    {
        var content = File.ReadAllText(filePath);
        Environment.SetEnvironmentVariable(environmentVariableName, content);
    }
}
