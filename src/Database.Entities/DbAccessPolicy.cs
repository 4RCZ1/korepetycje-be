using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(ResourceGroupId), nameof(StudentGroupId))]
[Table("access_policy")]
public class DbAccessPolicy : TenantEntity
{
    [Column("resource_group_id")]
    public int ResourceGroupId { get; set; }

    [Column("student_group_id")]
    public int StudentGroupId { get; set; }

    [ForeignKey(nameof(ResourceGroupId))]
    public DbResourceGroup? ResourceGroup { get; set; }

    [ForeignKey(nameof(StudentGroupId))]
    public DbStudentGroup? StudentGroup { get; set; }
}
