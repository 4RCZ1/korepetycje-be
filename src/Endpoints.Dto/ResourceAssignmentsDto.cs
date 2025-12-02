namespace Endpoints.Dto;

public class ResourceAssignmentsDto
{
    public required string resourdeGuid { get; set; }
    public required List<StudentDto> students { get; set; }
}