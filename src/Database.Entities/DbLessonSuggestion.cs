using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

[Table("lesson_suggestion")]
public class DbLessonSuggestion
{
    [Key]
    [Column("lesson_suggestion_id")]
    public int Id { get; set; }

    [Column("lesson_id")]
    public int? LessonId { get; set; }
    
    [Column("timeslot_id")]
    public int TimeslotId { get; set; }
    
    [Column("student_id")]
    public int StudentId { get; set; }
    
    [Column("address_id")]
    public int AddressId { get; set; }

    [ForeignKey(nameof(LessonId))]
    public DbLesson? Lesson { get; set; }
    
    [Required]
    [ForeignKey(nameof(TimeslotId))]
    public required DbTimeslot Timeslot { get; set; }
    
    [Required]
    [ForeignKey(nameof(StudentId))]
    public required DbStudent Student { get; set; }
    
    [Required]
    [ForeignKey(nameof(AddressId))]
    public required DbAddress Address { get; set; }
}
