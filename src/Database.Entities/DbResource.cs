using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("resource")]
public class DbResource : TenantEntity
{
    [Key]
    [Column("resource_id")]
    public int Id { get; set; }

    [Required]
    [Column("resource_guid")]
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(500)]
    [Column("file_path")]
    public required string Filename { get; set; }
}
