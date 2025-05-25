using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace HelloWorld;

public class Serie
{
    [Key]
    public int SeriesId { get; set; }
    [Required]
    public int StudentId { get; set; }
    [Required]
    public DateTime BeginTime { get; set; }
    [Required]
    public TimeSpan Offset { get; set; }
    [Required]
    public int EndOrdinal { get; set; }
    [Required]
    public TimeSpan LessonDuration { get; set; }
    
    [ForeignKey("Student_id")]
    [Required]
    public Student Student { get; set; }

    public List<Lesson>? Lessons { get; set; }
}