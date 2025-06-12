using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("schedule")]
public class DbSchedule
{
    [Key]
    [Column("schedule_id")]
    public int Id { get; set; }

    [Column("period")]
    public TimeSpan? Period { get; set; }

    public ICollection<DbLesson> Lessons { get; set; } = new List<DbLesson>();
}
