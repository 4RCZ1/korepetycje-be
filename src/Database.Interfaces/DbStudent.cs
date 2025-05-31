using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Models;

[Table("student")]
public class DbStudent
{
    [Key]
    [Column("student_id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("surname")]
    public required string Surname { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("address")]
    public required string Address { get; set; }

    public ICollection<DbSchedule> Schedules { get; } = new List<DbSchedule>();
}
