using Database.Entities;

namespace Services.Interfaces;

public interface IResourceStudentsDao
{
    ResourceStudentStructure GetResourceStudentStructure(int resourceId);
}

public class ResourceStudentStructure
{
    public List<DbStudent> DirectStudents { get; set; } = new();
    public List<DbStudentGroup> StudentGroups { get; set; } = new();
    public List<DbResourceGroup> ResourceGroups { get; set; } = new();
}