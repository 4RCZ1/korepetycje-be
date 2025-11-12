using Database.Entities;
using Services.Interfaces;

namespace Database;

internal class ResourceDao : IResourceDao
{
    public ResourceDao(TenantContext context)
    {
        _context = context;
    }

    public DbResource? GetResourceByGuid(Guid guid)
    {
        return _context.Resources.Query().SingleOrDefault(r => r.Guid == guid);
    }

    public void SaveSingleResource(string filePath, string singleGroupName)
    {
        _context.ResourceMemberships.Add(new DbResourceMembership
        {
            Resource = new DbResource { FilePath = filePath },
            Group = new DbResourceGroup
            {
                IsSingle = true,
                Name = singleGroupName,
            },
        });
    }

    private readonly TenantContext _context;
}
