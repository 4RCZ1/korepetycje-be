using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class TenantEntity
{
    [Required]
    [Column("tenant_id")]
    public int TenantId { get; set; }
}
