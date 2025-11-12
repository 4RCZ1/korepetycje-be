using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

[PrimaryKey(nameof(StudentId), nameof(GroupId))]
[Table("student_membership")]
public class DbStudentMembership : TenantEntity
{
    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("group_id")]
    public int GroupId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public DbStudent? Student { get; set; }

    [ForeignKey(nameof(GroupId))]
    public DbStudentGroup? Group { get; set; }
}
