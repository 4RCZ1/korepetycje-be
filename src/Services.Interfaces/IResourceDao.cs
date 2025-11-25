using Database.Entities;

namespace Services.Interfaces;

public interface IResourceDao
{
    DbResource? GetResourceByGuid(Guid guid);
    IList<DbResource> GetAllResources();
    void SaveSingleResource(string filename, string singleGroupName);
    void DeleteResource(DbResource resource);
    void DeleteGroup(DbResourceGroup group);
    DbResourceGroup GetResourceGroupByResourceId(int resourceId);
}
