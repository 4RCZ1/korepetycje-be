using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("resource_group")]
public class DbResourceGroup : TenantEntity
{
    [Key]
    [Column("resource_group_id")]
    public int Id { get; set; }

    [Required]
    [Column("resource_group_guid")]
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    [Column("is_single")]
    public required bool IsSingle { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public required string Name { get; set; }

    public ICollection<DbResourceMembership> Memberships { get; set; } =
        new List<DbResourceMembership>();
    
    public ICollection<DbAccessPolicy> AccessPolicies { get; set; } = new List<DbAccessPolicy>();
}
