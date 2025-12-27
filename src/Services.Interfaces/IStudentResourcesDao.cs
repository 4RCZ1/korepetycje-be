using Database.Entities;

namespace Services.Interfaces;

public interface IStudentResourcesDao
{
    StudentResourceStructure GetStudentResourceStructure(int studentId);
}
public class StudentResourceStructure
{
    public List<DbResource> DirectResources { get; set; } = new();
    public List<DbResourceGroup> ResourceGroups { get; set; } = new();
    public List<DbStudentGroup> StudentGroups { get; set; } = new();
}