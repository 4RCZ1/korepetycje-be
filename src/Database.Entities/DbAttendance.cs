using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(LessonId), nameof(StudentId), nameof(TenantId))]
[Table("attendance")]
public class DbAttendance : TenantEntity
{
    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("is_confirmed")]
    public bool? IsConfirmed { get; set; }

    [Required]
    [Column("has_occurred")]
    public bool HasOccurred { get; set; }

    [ForeignKey($"{nameof(StudentId)}, {nameof(TenantId)}")]
    public DbStudent? Student { get; set; }

    [ForeignKey($"{nameof(LessonId)}, {nameof(TenantId)}")]
    public DbLesson? Lesson { get; set; }
}
