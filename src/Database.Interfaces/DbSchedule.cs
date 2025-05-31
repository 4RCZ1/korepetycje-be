using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Interfaces;

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
    [Column("begin_time")]
    public DateTime BeginTime { get; set; }

    [Required]
    [Column("period")]
    public TimeSpan Period { get; set; }

    [Required]
    [Column("end_ordinal")]
    public int EndOrdinal { get; set; }

    [Required]
    [Column("lesson_duration")]
    public TimeSpan LessonDuration { get; set; }

    [ForeignKey(nameof(StudentId))]
    public required DbStudent Student { get; set; }

    public ICollection<DbLesson> Lessons { get; } = new List<DbLesson>();
}
