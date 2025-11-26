namespace Endpoints.Dto;

public class ResourceGroupDto
{
    public required string Guid { get; set; }
    
    public required bool IsSingle { get; set; }
    
    public required string Name { get; set; }
}