namespace HelloWorld.DTOs;

public class LessonDto
{
    public int ScheduleId { get; set; }

    public int Ordinal { get; set; }
    
    public TimeSpan? CustomDuration { get; set; }
    
    public string? TutorInfo { get; set; }
    
    public bool IsConfirmed { get; set; }
    
    public bool HasOccurred { get; set; }
}