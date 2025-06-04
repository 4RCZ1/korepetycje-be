using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("lesson")]
public class DbLesson
{
    [Key]
    [Column("lesson_id")]
    public int Id { get; set; }

    [Required]
    [Column("schedule_id")]
    public int ScheduleId { get; set; }
    
    [Required]
    [Column("timeslot_id")]
    public int? TimeslotId { get; set; }
    

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
    
    [ForeignKey(nameof(TimeslotId))]
    public required DbTimeslot Timeslot { get; set; }

    [ForeignKey(nameof(ScheduleId))]
    public DbSchedule? Schedule { get; set; }
}
