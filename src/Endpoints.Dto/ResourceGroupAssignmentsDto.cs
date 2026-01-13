namespace Endpoints.Dto;

public class ResourceGroupAssignmentsDto
{
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<ResourceDto>? Resources { get; set; } = new();
        public List<StudentDto>? DirectStudents { get; set; } = new();
        public List<StudentGroupDto>? StudentGroups { get; set; } = new();
}