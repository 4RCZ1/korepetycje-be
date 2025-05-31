namespace Timetable.Interfaces;

public class Lesson
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public required string TutorInfo { get; set; }
}
