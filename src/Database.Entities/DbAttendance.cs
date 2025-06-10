using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(LessonId), nameof(StudentId))]
[Table("attendance")]
public class DbAttendance
{
    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("student_id")]
    public int StudentId { get; set; }

    [Required]
    [Column("is_confirmed")]
    public bool IsConfirmed { get; set; }

    [Required]
    [Column("has_occurred")]
    public bool HasOccurred { get; set; }

    [ForeignKey(nameof(StudentId))]
    public DbStudent? Student { get; set; }

    [ForeignKey(nameof(LessonId))]
    public DbLesson? Lesson { get; set; }
}
