using Database.Entities;

namespace Services.Interfaces;

public interface IResourceDao
{
    DbResource? GetResourceByGuid(Guid guid);
    void SaveSingleResource(string filename, string singleGroupName);
}
