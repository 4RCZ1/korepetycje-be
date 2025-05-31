namespace Timetable.Interfaces;

public class LessonDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public required string Info { get; set; }
}
