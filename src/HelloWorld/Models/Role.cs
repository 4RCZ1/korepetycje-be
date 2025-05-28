using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Models;
[Table("roles")]
public class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("role_name")]
    [MaxLength(50)]
    public required string  RoleName { get; set; }

    public ICollection<User> Users { get; } = new List<User>();
}