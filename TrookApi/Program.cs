using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Services;
using TrookSii;
using TrookSii.Types.Raw;

var builder = WebApplication.CreateBuilder(args);

var sqliteDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "trook");
Directory.CreateDirectory(sqliteDirectory);
var sqliteFile = Path.Combine(sqliteDirectory, "trook.db");

builder.Services.AddDbContext<TrookDbContext>(o =>
{
    o.UseSqlite($"Data Source={sqliteFile}");
});

builder.Services.AddScoped<FileService>();

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TrookDbContext>();
    db.Database.Migrate();
    
    var fs = scope.ServiceProvider.GetRequiredService<FileService>();
    await fs.ReadAndSaveFileAsync("testsave_withjobs.sii");
}

app.Run();