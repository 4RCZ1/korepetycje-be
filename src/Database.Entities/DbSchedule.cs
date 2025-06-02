using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("schedule")]
public class DbSchedule
{
    [Key]
    [Column("schedule_id")]
    public int Id { get; set; }

    [Required]
    [Column("student_id")]
    public int StudentId { get; set; }

    [Required]
    [Column("period")]
    public TimeSpan Period { get; set; }

    [Required]
    [Column("lesson_duration")]
    public TimeSpan LessonDuration { get; set; }

    [ForeignKey(nameof(StudentId))]
    public DbStudent? Student { get; set; }

    public ICollection<DbLesson> Lessons { get; set; } = new List<DbLesson>();
}
