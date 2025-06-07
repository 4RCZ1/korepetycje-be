using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;
[Table("attendance")]
public class DbAttendance
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("lesson_id")]
    public int LessonId { get; set; }
    
    [Required]
    [Column("student_id")]
    public int StudentId { get; set; }
    
    [Required]
    [Column("is_confirmed")]
    public bool IsConfirmed { get; set; }

    [Required]
    [Column("has_occurred")]
    public bool HasOccurred { get; set; }
    
    [Required]
    [ForeignKey(nameof(StudentId))]
    public required DbStudent Student { get; set; }
    
    [Required]
    [ForeignKey(nameof(LessonId))]
    public required DbLesson Lesson { get; set; }
    
}