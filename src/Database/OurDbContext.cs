using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class OurDbContext : DbContext
{
    public OurDbContext(string connection)
    {
        _connection = connection;
    }

    public OurDbContext()
        : this(string.Empty)
    {
    }

    public DbSet<DbLesson> Lessons { get; set; }
    public DbSet<DbStudent> Students { get; set; }
    public DbSet<DbSchedule> Schedules { get; set; }
    public DbSet<DbTimeslot> Timeslots { get; set; }
    public DbSet<DbLessonSuggestion> LessonSuggestions { get; set; }
    public DbSet<DbAttendance> Attendances { get; set; }
    public DbSet<DbAddress> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_connection)
            .AddInterceptors(new SoftDeleteInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbStudent>()
            .HasQueryFilter(x => x.IsDeleted == false);
    }

    private readonly string _connection;
}
