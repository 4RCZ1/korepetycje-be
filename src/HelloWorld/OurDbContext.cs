using Microsoft.EntityFrameworkCore;
using HelloWorld.Models;
namespace HelloWorld;

public class OurDbContext : DbContext
{
    public OurDbContext(DbContextOptions<OurDbContext> options)
        : base(options) { }
    public DbSet<DbLesson> Lessons { get; set; }
    public DbSet<DbStudent> Students { get; set; }
    public DbSet<DbSchedule> Schedules { get; set; }
}
