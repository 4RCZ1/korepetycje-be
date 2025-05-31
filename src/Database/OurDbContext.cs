using Database.Interfaces;
using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class OurDbContext : DbContext
{
    public OurDbContext(DbContextOptions<OurDbContext> options)
        : base(options) { }
    public DbSet<DbLesson> Lessons { get; set; }
    public DbSet<DbStudent> Students { get; set; }
    public DbSet<DbSchedule> Schedules { get; set; }
}
