using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("timeslots")]
public class DbTimeslot
{
    [Key]
    [Column("timeslot_id")]
    public int Id { get; set; }

    [Required]
    [Column("is_free")]
    public bool IsFree { get; set; } // todo: reconsider

    [Required]
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Required]
    [Column("end_time")]
    public DateTime EndTime { get; set; }
}
