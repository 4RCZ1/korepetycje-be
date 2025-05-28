using Microsoft.EntityFrameworkCore;
using HelloWorld.Models;
namespace HelloWorld;

public class OurDbContext : DbContext
{
    public OurDbContext(DbContextOptions<OurDbContext> options)
        : base(options) { }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
}
