using Database.Entities;

namespace Services.Interfaces;

public interface IResourceDao
{
    DbResource? GetResourceByGuid(Guid guid);
    IList<DbResource> GetAllResources();
    IList<DbResourceGroup> GetAllResourceGroups();
    void SaveSingleResource(string filename, string singleGroupName);
    void DeleteResource(DbResource resource);
    void DeleteGroup(DbResourceGroup group);
    DbResourceGroup GetResourceSingleGroupByResourceId(int resourceId);
}
