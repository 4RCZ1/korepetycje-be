namespace Endpoints.Dto;

public class MultiAssignmentDto
{
    public required IList<Guid>? ResourceIds { get; set; }
    public required IList<Guid>? ResourceGroupIds { get; set; }
    public required IList<string>? StudentIds { get; set; }
    public required IList<Guid>? StudentGroupIds { get; set; }
}
