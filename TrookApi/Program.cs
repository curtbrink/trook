using System.Diagnostics;
using TrookSii;
using TrookSii.Types.Mappings;
using TrookSii.Types.Models;

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

var clock = Stopwatch.StartNew();
var t = await SiiDecryptor.DecryptScsc(File.ReadAllBytes("testsave_withjobs.sii"));
var decodedFile = SiiDecoder.DecodeSii(t);
clock.Stop();
Console.WriteLine($"Decryption, decompression, and decoding completed in {clock.ElapsedMilliseconds} ms");

Console.WriteLine($"decoded file has {decodedFile.Structures.Count} structure blocks!");
Console.WriteLine($"decoded file has {decodedFile.Data.Count} (generic untyped) data blocks and many more typed ones!");

// list economy fields
var str = decodedFile.Data.First();
foreach (var (vd, v) in str.Data)
{
    Console.WriteLine($"{vd.Name} => {v.ToString()}");
}

// hierarchy test?
var clock2 = Stopwatch.StartNew();
foreach (var ai in decodedFile.AiDriverBlocks)
{
    if (ai.ProfitLog.IsEmpty) continue;
    
    var profitLog = decodedFile.ProfitLogBlocks.FirstOrDefault(pl => pl.BlockId.Key == ai.ProfitLog.Key);
    if (profitLog == null || profitLog.StatsDataBlockIds.Length == 0) continue;

    Console.WriteLine(
        $"{ai.BlockId.Key} has a profit log with {profitLog.StatsDataBlockIds.Length} data blocks...");

    foreach (var dataBlockId in profitLog.StatsDataBlockIds)
    {
        var profitLogEntry = decodedFile.ProfitLogEntryBlocks.FirstOrDefault(pl => pl.BlockId.Key == dataBlockId.Key);
        if (profitLogEntry == null) continue;

        Console.WriteLine(
            $"=> {profitLogEntry.Cargo} from {profitLogEntry.SourceCity} to {profitLogEntry.DestinationCity}");
    }
}
clock2.Stop();

Console.WriteLine($"traversed driver/profit log hierarchy in {clock2.ElapsedMilliseconds} ms");