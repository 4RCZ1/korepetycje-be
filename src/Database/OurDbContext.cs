using Database.Interfaces;
using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class OurDbContext : DbContext
{
    public DbSet<DbLesson> Lessons { get; set; }
    public DbSet<DbStudent> Students { get; set; }
    public DbSet<DbSchedule> Schedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql();
    }
}
