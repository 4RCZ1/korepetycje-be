namespace HelloWorld.DTOs;

public class ScheduleDto
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public DateTime BeginTime { get; set; }

    public TimeSpan Period { get; set; }

    public int EndOrdinal { get; set; }

    public TimeSpan LessonDuration { get; set; }

    public List<int> LessonIds { get; set; } = new List<int>();
}