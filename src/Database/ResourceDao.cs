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

    public IList<DbResource> GetAllResources()
    {
        return _context.Resources.Query().ToList();
    }

    public void SaveSingleResource(string filename, string singleGroupName)
    {
        _context.ResourceMemberships.Add(new DbResourceMembership
        {
            Resource = new DbResource { Filename = filename },
            Group = new DbResourceGroup
            {
                IsSingle = true,
                Name = singleGroupName,
            },
        });
    }

    private readonly TenantContext _context;
}
