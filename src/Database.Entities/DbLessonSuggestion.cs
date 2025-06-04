using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;
[Table ("lesson_suggestions")]

public class DbLessonSuggestion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("suggested_start")]
    public DateTime SuggestedStart { get; set; }
    
    [Required]
    [Column("suggested_end")]
    public DateTime SuggestedEnd { get; set; }
    
    [Column("lesson_id")]
    public int? LessonId { get; set; }
    
    [ForeignKey(nameof(LessonId))]
    public DbLesson? Lesson { get; set; }
}