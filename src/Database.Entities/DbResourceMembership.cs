using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(ResourceId), nameof(GroupId))]
[Table("resource_membership")]
public class DbResourceMembership : TenantEntity
{
    [Column("resource_id")]
    public int ResourceId { get; set; }

    [Column("group_id")]
    public int GroupId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public DbResource? Resource { get; set; }

    [ForeignKey(nameof(GroupId))]
    public DbResourceGroup? Group { get; set; }
}
