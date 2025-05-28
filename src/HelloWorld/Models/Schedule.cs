using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Models;

[Table("schedule")]
public class Schedule
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
    public required Student Student { get; set; }

    public ICollection<Lesson> Lessons { get; } = new List<Lesson>();
}
