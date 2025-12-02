namespace Endpoints.Dto;

public class StudentAssignmentsDto
{
    public string StudentId { get; set; }
    public List<ResourceDto> Resources { get; set; }
}