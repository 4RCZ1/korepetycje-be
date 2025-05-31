namespace Timetable.Interfaces;

public class LessonDto
{
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Info { get; set; }
}
