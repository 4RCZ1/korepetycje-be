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

    public void DeleteGroup(DbResourceGroup group)
    {
        _context.ResourceGroups.Remove(group);
    }

    public DbResourceGroup GetResourceSingleGroupByResourceId(int resourceId)
    {
        var group = _context.ResourceMemberships.Query()
            .Where(m => m.ResourceId == resourceId)
            .Select(m => m.Group)
            .FirstOrDefault(g => g != null && g.IsSingle);
        if(group == null)
            throw new ApplicationException("Single resource group not found");
        return group;
    }
    
    public DbResourceGroup GetResourceGroupById(Guid resourceGroupId)
    {
        var group = _context.ResourceGroups.Query()
            .Where(r => r.Guid == resourceGroupId)?
            .FirstOrDefault();
        if (group == null)
            throw new ApplicationException("Resource group not found");
        return group;
    }

    private readonly TenantContext _context;
}
