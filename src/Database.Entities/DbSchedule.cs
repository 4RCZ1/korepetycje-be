using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("schedule")]
public class DbSchedule : TenantEntity
{
    [Key]
    [Column("schedule_id")]
    public int Id { get; set; }

    [Required]
    [Column("address_id")]
    public int AddressId { get; set; }

    [Column("period")]
    public TimeSpan? Period { get; set; }

    [ForeignKey(nameof(AddressId))]
    public DbAddress? Address { get; set; }

    public ICollection<DbLesson> Lessons { get; set; } = new List<DbLesson>();
}
