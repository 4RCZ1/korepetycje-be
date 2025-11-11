using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("timeslot")]
public class DbTimeslot : TenantEntity
{
    [Key]
    [Column("timeslot_id")]
    public int Id { get; set; }

    [Required]
    [Column("start_time")]
    public DateTimeOffset StartTime { get; set; }

    [Required]
    [Column("end_time")]
    public DateTimeOffset EndTime { get; set; }
}
