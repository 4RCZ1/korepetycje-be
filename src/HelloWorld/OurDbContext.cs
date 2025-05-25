using Microsoft.EntityFrameworkCore;

namespace HelloWorld;

public class OurDbContext : DbContext
{
    public OurDbContext(DbContextOptions<OurDbContext> options)
        : base(options) { }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Serie> Series { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>()
            .HasKey(l => new { l.Series_id, l.Ordinal });

        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Serie)
            .WithMany(s => s.Lessons)
            .HasForeignKey(l => l.Series_id);

        modelBuilder.Entity<Lesson>()
            .Property(l => l.Is_confirmed)
            .IsRequired();

        modelBuilder.Entity<Lesson>()
            .Property(l => l.Has_occured)
            .IsRequired();

        modelBuilder.Entity<Serie>()
            .HasKey(s => s.SeriesId);
        
        modelBuilder.Entity<Serie>()
            .HasOne(s => s.Student)
            .WithMany(s => s.Series)
            .HasForeignKey(s => s.StudentId);
        
        modelBuilder.Entity<Serie>()
            .Property(s => s.StudentId)
            .IsRequired();
        
        modelBuilder.Entity<Serie>()
            .Property(s => s.BeginTime)
            .IsRequired();
        
        modelBuilder.Entity<Serie>()
            .Property(s => s.Offset)
            .IsRequired();
        
        modelBuilder.Entity<Serie>()
            .Property(s => s.EndOrdinal)
            .IsRequired();
        
        modelBuilder.Entity<Serie>()
            .Property(s => s.LessonDuration)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .HasKey(s => s.Id);
        
        modelBuilder.Entity<Student>()
            .Property(s => s.Name)
            .IsRequired();
        
        modelBuilder.Entity<Student>()
            .Property(s => s.Surname)
            .IsRequired();
        
        modelBuilder.Entity<Student>()
            .Property(s=> s.Address)
            .IsRequired();
    }
}