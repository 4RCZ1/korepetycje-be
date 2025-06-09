using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;
[Table("address")]
public class DbAddress
{
    [Key]
    [Column("id")]
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