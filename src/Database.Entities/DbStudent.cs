using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("student")]
public class DbStudent : ISoftDelete
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
    
    [Column("address_id")]
    public int? AddressId { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public ICollection<DbSchedule> Schedules { get; } = new List<DbSchedule>();
    
    [ForeignKey(nameof(AddressId))]
    public DbAddress? Address { get; set; }
}
