using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(Id), nameof(TenantId))]
[Table("address")]
public class DbAddress : TenantEntity
{
    [Column("address_id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("address_name")]
    public required string AddressName { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("address_data")]
    public required string AddressData { get; set; }
}
