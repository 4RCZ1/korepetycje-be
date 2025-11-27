namespace Endpoints.Dto;

public class ResourceGroupDto
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required IList<ResourceDto> Resources { get; set; }
}
