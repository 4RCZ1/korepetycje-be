using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(Id), nameof(TenantId))]
[Table("lesson_suggestion")]
public class DbLessonSuggestion : TenantEntity
{
    [Column("lesson_suggestion_id")]
    public int Id { get; set; }

    [Column("lesson_id")]
    public int? LessonId { get; set; }

    [Required]
    [Column("timeslot_id")]
    public int TimeslotId { get; set; }

    [Required]
    [Column("student_id")]
    public int StudentId { get; set; }

    [Required]
    [Column("address_id")]
    public int AddressId { get; set; }

    [ForeignKey($"{nameof(LessonId)}, {nameof(TenantId)}")]
    public DbLesson? Lesson { get; set; }

    [ForeignKey($"{nameof(TimeslotId)}, {nameof(TenantId)}")]
    public DbTimeslot? Timeslot { get; set; }

    [ForeignKey($"{nameof(StudentId)}, {nameof(TenantId)}")]
    public DbStudent? Student { get; set; }

    [ForeignKey($"{nameof(AddressId)}, {nameof(TenantId)}")]
    public DbAddress? Address { get; set; }
}
