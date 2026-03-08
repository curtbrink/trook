using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Services;
using TrookSii.Types.Raw;

var builder = WebApplication.CreateBuilder(args);

const int apiPort = 56277;

// ===== configure database =====

var sqliteDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "trook");
Directory.CreateDirectory(sqliteDirectory);
var sqliteFile = Path.Combine(sqliteDirectory, "trook.db");

builder.Services.AddDbContext<TrookDbContext>(o =>
{
    o.UseSqlite($"Data Source={sqliteFile}");
});

// ===== configure services =====

builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<DriverJobService>();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

// ===== configure middleware =====

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

// ===== migrate database =====

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TrookDbContext>();
    db.Database.Migrate();
}

// ===== open the browser when the app is ready =====

app.Lifetime.ApplicationStarted.Register(() =>
{
    // open the vite url if dev
    var port = app.Environment.IsDevelopment() ? 56279 : apiPort;
    var url = $"http://localhost:{port}";
    // Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    
    using var scope = app.Services.CreateScope();
    var fs = scope.ServiceProvider.GetRequiredService<FileService>();
    var djs = scope.ServiceProvider.GetRequiredService<DriverJobService>();
    var t = fs.ReadAndSaveFileAsync("game_wjobs.sii");
    Task.WaitAll(t);
    var sii = t.Result;
    if (sii != null && sii is SiiBinaryFile sbf)
    {
        var t2 = djs.ExtractDriverJobs(sbf);
        Task.WaitAll(t2);
    }
});

app.Run($"http://localhost:{apiPort}");