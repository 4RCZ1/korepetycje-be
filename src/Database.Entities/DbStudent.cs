using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(Id), nameof(TenantId))]
[Table("student")]
public class DbStudent : TenantEntity, ISoftDelete
{
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

    [Required]
    [MaxLength(20)]
    [Column("phone_number")]
    public required string PhoneNumber { get; set; }

    [Column("address_id")]
    public int? AddressId { get; set; }

    [Required]
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTimeOffset? DeletedAt { get; set; }

    [ForeignKey($"{nameof(AddressId)}, {nameof(TenantId)}")]
    public DbAddress? Address { get; set; }
}
