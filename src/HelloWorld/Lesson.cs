using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld;

[PrimaryKey(nameof(ScheduleId), nameof(Ordinal))]
[Table("lesson")]
public class Lesson
{
    [Key]
    [Column("schedule_id")]
    public int ScheduleId { get; set; }

    [Key]
    [Column("ordinal")]
    public int Ordinal { get; set; }

    [Column("custom_duration")]
    public TimeSpan? CustomDuration { get; set; }

    [MaxLength(200)]
    [Column("tutor_info")]
    public string? TutorInfo { get; set; }

    [Required]
    [Column("is_confirmed")]
    public bool IsConfirmed { get; set; }

    [Required]
    [Column("has_occurred")]
    public bool HasOccurred { get; set; }

    [ForeignKey(nameof(ScheduleId))]
    public required Schedule Schedule { get; set; }
}
