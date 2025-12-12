namespace Endpoints.Dto;

public class StudentGroupDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public required IList<StudentDto> Students { get; set; }
}
