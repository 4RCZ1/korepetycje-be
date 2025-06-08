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

    public ILessonDao LessonDao { get; }

    public void Commit()
    {
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_connection);
    }

    private readonly string _connection;
}
