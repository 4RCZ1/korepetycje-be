using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Timetable.Interfaces;

namespace Database;

public class OurDbContext : DbContext, ITransaction
{
    public OurDbContext(string connection)
    {
        _connection = connection;
        LessonDao = new LessonDao(this);
        StudentDao = new StudentDao(this);
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

    public ILessonDao LessonDao { get; }
    public IStudentDao StudentDao { get; }

    public void Commit()
    {
        SaveChanges();
    }

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
