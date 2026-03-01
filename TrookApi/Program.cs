using TrookApi.Mappings;
using TrookSii;

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

// ============== do stuff

var t = await SiiDecryptor.DecryptScsc(File.ReadAllBytes("testsave_withjobs.sii"));
var decodedFile = SiiDecoder.DecodeSii(t);

Console.WriteLine($"decoded file has {decodedFile.Structures.Count} structure blocks!");
Console.WriteLine($"decoded file has {decodedFile.Data.Count} data blocks!");

var profitLogEntries = decodedFile.Data.Where(db => db.StructureId == 15).ToList();
foreach (var entry in profitLogEntries)
{
    var v = entry.ToProfitLogEntry();
}
