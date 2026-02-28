using TrookSii;
using TrookSii.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

var t = await SiiDecryptor.DecryptScsc(File.ReadAllBytes("testsave_withjobs.sii"));
var decodedFile = SiiDecoder.DecodeSii(t);

// sort blocks into structures
var dict = new Dictionary<uint, List<DataBlock>>();
foreach (var db in decodedFile.Data)
{
    if (!dict.ContainsKey(db.StructureId))
        dict[db.StructureId] = [];

    dict[db.StructureId].Add(db);
}

foreach (var k in dict.Keys)
{
    var s = decodedFile.Structures[k];
    Console.WriteLine($"Structure block '{s.Name}' accounts for {dict[k].Count} data blocks");
}

Console.WriteLine($"decoded file has {decodedFile.Structures.Count} structure blocks!");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}