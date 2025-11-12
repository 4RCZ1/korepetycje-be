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
    public DbSet<DbTutor> Tutors { get; set; }
    public DbSet<DbResource> Resources { get; set; }
    public DbSet<DbResourceGroup> ResourceGroups { get; set; }
    public DbSet<DbResourceMembership> ResourceMemberships { get; set; }
    public DbSet<DbStudentGroup> StudentGroups { get; set; }
    public DbSet<DbStudentMembership> StudentMemberships { get; set; }
    public DbSet<DbAccessPolicy> AccessPolicies { get; set; }

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
