using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Models;
[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("user_name")]
    [MaxLength(50)]
    public required string UserName { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Column("password")]
    public required string Password { get; set; }
    
    [Required]
    [Column("role_id")]
    public required int RoleId { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    public required Role Role { get; set; }
}