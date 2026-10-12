using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database: connection string comes from ConnectionStrings__Default (env var)
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// CORS: allowed frontend origin comes from Cors__AllowedOrigin (env var)
var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"] ?? "http://localhost:5173";
builder.Services.AddCors(o => o.AddPolicy("Frontend", p =>
    p.WithOrigins(allowedOrigin)
     .AllowAnyHeader()
     .AllowAnyMethod()));

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