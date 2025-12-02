using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("student_group")]
public class DbStudentGroup : TenantEntity
{
    [Key]
    [Column("student_group_id")]
    public int Id { get; set; }

    [Required]
    [Column("student_group_guid")]
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    [Column("is_single")]
    public required bool IsSingle { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public required string Name { get; set; }
    
    public ICollection<DbAccessPolicy> AccessPolicies { get; set; } = new List<DbAccessPolicy>();
    public ICollection<DbStudentMembership> Memberships { get; set; } =
        new List<DbStudentMembership>();
}
