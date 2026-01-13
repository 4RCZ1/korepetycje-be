namespace Endpoints.Dto;

public class ResourceStudentsResponse
{
    public class ResourceWithStudentsResponse
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<StudentDto>? DirectStudents { get; set; } = new();
        public List<StudentGroupWithStudentsDto>? StudentGroups { get; set; } = new();
        public List<ResourceGroupWithStudentsDto>? ResourceGroups { get; set; } = new();
    }

    public class StudentGroupWithStudentsDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public bool IsSingle { get; set; }
        public List<StudentDto>? Students { get; set; } = new();
    }

    public class ResourceGroupWithStudentsDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public bool IsSingle { get; set; }
        public List<StudentDto>? DirectStudents { get; set; } = new();
        public List<StudentGroupWithStudentsDto>? StudentGroups { get; set; } = new();
    }
}