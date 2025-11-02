using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(Id), nameof(TenantId))]
[Table("lesson")]
public class DbLesson : TenantEntity
{
    [Column("lesson_id")]
    public int Id { get; set; }

    [Required]
    [Column("schedule_id")]
    public int ScheduleId { get; set; }

    [Required]
    [Column("timeslot_id")]
    public int TimeslotId { get; set; }

    [MaxLength(200)]
    [Column("tutor_info")]
    public string? TutorInfo { get; set; }

    [ForeignKey($"{nameof(TimeslotId)}, {nameof(TenantId)}")]
    public DbTimeslot? Timeslot { get; set; }

    [ForeignKey($"{nameof(ScheduleId)}, {nameof(TenantId)}")]
    public DbSchedule? Schedule { get; set; }

    public ICollection<DbAttendance> Attendances { get; set; } = new List<DbAttendance>();
}
