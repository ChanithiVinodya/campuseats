using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database: connection string comes from ConnectionStrings__Default (env var)
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// CORS: allow configured origins (e.g. Vercel) and localhost for testing
var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"];
var origins = (allowedOrigin ?? "")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(o => o.AddPolicy("Frontend", p =>
{
    if (origins.Length > 0)
    {
        p.WithOrigins(origins);
    }

    p.SetIsOriginAllowed(origin =>
    {
        if (string.IsNullOrEmpty(origin)) return false;
        try
        {
            var uri = new Uri(origin);
            return uri.Host is "localhost" or "127.0.0.1";
        }
        catch
        {
            return false;
        }
    })
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

var app = builder.Build();

app.UseCors("Frontend");   // before the endpoints

app.MapGet("/", () => "CampusEats API is running");
app.MapGet("/health", () => Results.Ok("healthy"));

app.MapGet("/api/menu", async (AppDbContext db, ILogger<Program> log) =>
{
    var items = await db.MenuItems.ToListAsync();
    log.LogInformation("Menu requested, returning {Count} items", items.Count);
    return Results.Ok(items);
});

app.Run();