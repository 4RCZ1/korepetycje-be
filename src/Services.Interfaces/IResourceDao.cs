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
    void EmptyResourceGroup(int groupId);
    void DeleteGroup(DbResourceGroup group);
    DbResourceGroup GetResourceSingleGroupByResourceId(int resourceId);
    IList<DbResource> GetStudentResources(int studentId);
    DbResourceGroup? GetResourceGroupByGuid(Guid groupGuid);
    void SaveAccessPolicyIfNotExists(int studentGroupId, int resourceGroupId);
    void DeleteAccessPolicy(int studentGroupId, int resourceGroupId);
}
