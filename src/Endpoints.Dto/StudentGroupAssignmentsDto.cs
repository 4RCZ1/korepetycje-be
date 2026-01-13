namespace Endpoints.Dto;

public class StudentGroupAssignmentsDto
{
    public string Id { get; set; }
    public required string Name { get; set; }

    public List<ResourceDto> DirectResources { get; set; } = new();
    public List<ResourceGroupDto> ResourceGroups { get; set; } = new();
    public List<StudentDto> Students { get; set; } = new();
}