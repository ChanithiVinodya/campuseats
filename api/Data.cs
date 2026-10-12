using Microsoft.EntityFrameworkCore;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Chicken Rice", Price = 650m },
            new MenuItem { Id = 2, Name = "Kottu Roti",   Price = 800m },
            new MenuItem { Id = 3, Name = "Veg Wrap",     Price = 450m },
            new MenuItem { Id = 4, Name = "Iced Coffee",  Price = 300m }
        );
    }
}