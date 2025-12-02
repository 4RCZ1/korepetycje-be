using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore; 

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
    
    public IList<DbResourceGroup> GetAllResourceGroups()
    {
        return _context.ResourceGroups.Query()
            .Include(g => g.Memberships)
                .ThenInclude(m => m.Resource)
            .Where(g => !g.IsSingle)
            .ToList();
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

    public void SaveResourceGroup(DbResourceGroup group)
    {
        _context.ResourceGroups.Add(group);
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
    
    public IList<DbResource> GetStudentResources(int studentId)
    {
        return _context.Resources.Query()
            .Include(r => r.Memberships)
                .ThenInclude(rm => rm.Group)
                    .ThenInclude(rg => rg!.AccessPolicies)
                        .ThenInclude(ap => ap.StudentGroup)
                            .ThenInclude(sg => sg!.Memberships)
            .Where(resource => resource.Memberships
                .Any(rm => rm.Group != null && rm.Group.AccessPolicies
                    .Any(ap => ap.StudentGroup != null && ap.StudentGroup.Memberships
                        .Any(sm => sm.StudentId == studentId))))

            .ToList();
    }

    private readonly TenantContext _context;
}
