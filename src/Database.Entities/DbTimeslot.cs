using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(Id), nameof(TenantId))]
[Table("timeslot")]
public class DbTimeslot : TenantEntity
{
    [Column("timeslot_id")]
    public int Id { get; set; }

    [Required]
    [Column("start_time")]
    public DateTimeOffset StartTime { get; set; }

    [Required]
    [Column("end_time")]
    public DateTimeOffset EndTime { get; set; }
}
