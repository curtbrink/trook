using System.Diagnostics;
using TrookSii;
using TrookSii.Types.Raw;

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

Console.WriteLine($"decoded file has {decodedFile.StructureCount} structure blocks!");
Console.WriteLine($"decoded file has {decodedFile.DataCount} data blocks!");

// list economy fields
var econ = decodedFile.GetDataByStructureName("economy").First();
var player = decodedFile.GetData(econ.GetValue<BlockId>("player").Key);

Console.WriteLine("Foo!");