namespace HelloWorld.DTOs;

public class StudentDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Surname { get; set; }

    public required string Address { get; set; }

    public List<int> ScheduleIds { get; set; } = new List<int>();
}