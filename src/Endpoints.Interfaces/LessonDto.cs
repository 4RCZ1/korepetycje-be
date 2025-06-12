namespace Endpoints.Interfaces;

public class LessonDto
{
    public required string LessonId { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Address { get; set; } // todo: replace with DTO
    public required string Description { get; set; }
    public required IList<AttendanceDto> Attendances { get; set; }
}
