using Database.Entities;

namespace Services.Interfaces;

public interface IResourceDao
{
    DbResource? GetResourceByGuid(Guid guid);
    IList<DbResource> GetAllResources();
    IList<DbResourceGroup> GetAllResourceGroups();
    void SaveSingleResource(string filename, string singleGroupName);
    void DeleteResource(DbResource resource);
    void DeleteGroupByGuid(Guid groupId);
    DbResourceGroup GetResourceGroupById(Guid resourceGroupId);
    void SaveResourceGroup(DbResourceGroup group);
    DbResourceGroup GetResourceSingleGroupByResourceId(int resourceId);
    IList<DbResource> GetStudentResources(int studentId);
    IList<DbStudent> GetResourceAssignments(Guid resourceId);
}
