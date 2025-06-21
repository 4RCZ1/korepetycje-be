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

    [Required]
    [Column("timeslot_id")]
    public int TimeslotId { get; set; }

    [Required]
    [Column("student_id")]
    public int StudentId { get; set; }

    [Required]
    [Column("address_id")]
    public int AddressId { get; set; }

    [ForeignKey(nameof(LessonId))]
    public DbLesson? Lesson { get; set; }

    [ForeignKey(nameof(TimeslotId))]
    public DbTimeslot? Timeslot { get; set; }

    [ForeignKey(nameof(StudentId))]
    public DbStudent? Student { get; set; }

    [ForeignKey(nameof(AddressId))]
    public DbAddress? Address { get; set; }
}
