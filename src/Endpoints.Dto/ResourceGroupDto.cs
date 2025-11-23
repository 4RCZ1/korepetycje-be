namespace Endpoints.Dto;

public class ResourceGroupDto
{
    public required string Name { get; set; }
    public required IList<ResourceDto> Resources { get; set; }
}
