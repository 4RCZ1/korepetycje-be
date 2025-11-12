using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(TenantId))]
[Table("tutor")]
public class DbTutor : TenantEntity
{
    [Required]
    [Column("resource_path_prefix")]
    public required string ResourcePathPrefix { get; set; }
}
