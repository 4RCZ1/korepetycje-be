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
    
    public void DeleteResource(DbResource resource)
    {
        _context.Resources.Remove(resource);
    }

    public void DeleteSingleGroup(string groupName)
    {
        _context.ResourceGroups.Remove(GetResourceGroupByName(groupName));
    }

    private DbResourceGroup GetResourceGroupByName(string groupName)
    {
        var group = _context.ResourceGroups.Query()?.Where(g => g.Name == groupName).FirstOrDefault();
        if(group == null)
            throw new FileNotFoundException("No such resource group");
        return group;
    }

    private readonly TenantContext _context;
}
