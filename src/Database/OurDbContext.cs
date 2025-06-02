using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class OurDbContext : DbContext
{
    public DbSet<DbLesson> Lessons { get; set; }
    public DbSet<DbStudent> Students { get; set; }
    public DbSet<DbSchedule> Schedules { get; set; }

    public OurDbContext(string connection)
    {
        _connection = connection;
    }

    public OurDbContext()
    {
        _connection = "";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_connection);
    }

    private readonly string _connection;
}
