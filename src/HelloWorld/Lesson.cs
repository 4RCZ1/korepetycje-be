using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace HelloWorld;

public class Lesson
{
     [Key]
     [Column(Order = 1)]
     public int Series_id { get; set; }
     [Key]
     [Column(Order = 2)]
     public int Ordinal { get; set; }
     public DateTime Custom_time { get; set; }
     public TimeSpan Custom_duration { get; set; }
     public string Tutor_info { get; set; }
     [Required]
     public bool Is_confirmed { get; set; }
     [Required]
     public bool Has_occured { get; set; }
     
     [ForeignKey("Series_id")]
     public Serie Serie { get; set; }
}