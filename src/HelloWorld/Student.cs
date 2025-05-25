using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld;

public class Student
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public string Address { get; set; }
    
    public List<Serie>? Series { get; set; }
}