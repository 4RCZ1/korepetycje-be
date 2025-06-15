namespace Endpoints.Interfaces;

public class AttendanceDto
{
    public required string StudentName { get; set; }
    public required string StudentSurname { get; set; }
    public required bool? Confirmed { get; set; }
}
