namespace Endpoints.Dto;

public class StudentResourcesResponse
{
    public class StudentWithResourcesResponse
    {
        public int StudentId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }

        public List<ResourceDto> DirectResources { get; set; } = new();
        public List<ResourceGroupWithResourcesDto> ResourceGroups { get; set; } = new();
        public List<StudentGroupWithResourcesDto> StudentGroups { get; set; } = new();
    }

    public class ResourceGroupWithResourcesDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public bool IsSingle { get; set; }
        public List<ResourceDto> Resources { get; set; } = new();
    }

    public class StudentGroupWithResourcesDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public bool IsSingle { get; set; }
        public List<ResourceDto> DirectResources { get; set; } = new();
        public List<ResourceGroupWithResourcesDto> ResourceGroups { get; set; } = new();
    }
}