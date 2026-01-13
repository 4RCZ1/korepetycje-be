namespace Endpoints.Dto;

public class StudentGroupDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public IList<string>? StudentIds { get; set; }
    public IList<StudentDto>? Students { get; set; }
}
