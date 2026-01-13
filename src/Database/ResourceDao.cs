using Database.Entities;
using Microsoft.EntityFrameworkCore;
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
        _context.ResourceGroups.Update(group);
    }

    public void EmptyResourceGroup(int groupId)
    {
        _context.ResourceMemberships.RemoveRange(
            _context.ResourceMemberships.Query().Where(m => m.GroupId == groupId));
    }

    public void DeleteGroupByGuid(Guid groupId)
    {
        var group = _context.ResourceGroups.Query().SingleOrDefault(r => r.Guid == groupId);
        if (group == null)
            return;
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

    public IList<DbStudent> GetResourceAssignments(Guid resourceId)
    {
        return _context.Students.Query()
            .Include(s => s.Memberships)
            .ThenInclude(sm => sm.Group)
            .ThenInclude(sg => sg!.AccessPolicies)
            .ThenInclude(ap => ap.ResourceGroup)
            .ThenInclude(rg => rg!.Memberships)
            .ThenInclude(rm => rm.Resource)
            .Where(s => s.Memberships
                .Any(sm => sm.Group != null && sm.Group.AccessPolicies
                    .Any(ap => ap.ResourceGroup != null && ap.ResourceGroup.Memberships
                        .Any(rm => rm.Resource !=null && rm.Resource.Guid == resourceId))))
            .ToList();
     }


    public DbResourceGroup? GetResourceGroupByGuid(Guid groupGuid)
    {
        return _context.ResourceGroups.Query().SingleOrDefault(g => g.Guid == groupGuid);
    }

    public void SaveAccessPolicyIfNotExists(int studentGroupId, int resourceGroupId)
    {
        var policy = _context.AccessPolicies.Query().SingleOrDefault(
            p => p.StudentGroupId == studentGroupId && p.ResourceGroupId == resourceGroupId);
        if (policy == null)
        {
            _context.AccessPolicies.Add(new DbAccessPolicy
            {
                StudentGroupId = studentGroupId,
                ResourceGroupId = resourceGroupId,
            });
        }
    }

    public void DeleteAccessPolicy(int studentGroupId, int resourceGroupId)
    {
        var policy = _context.AccessPolicies.Query().SingleOrDefault(
            p => p.StudentGroupId == studentGroupId && p.ResourceGroupId == resourceGroupId);
        if (policy != null)
            _context.AccessPolicies.Remove(policy);
    }

    public bool StudentHasAccessToResource(int studentId, Guid resourceGuid)
    {
        return _context.AccessPolicies.Query().Any(p =>
            p.ResourceGroup!.Memberships.Any(rm => rm.Resource!.Guid == resourceGuid)
            && p.StudentGroup!.Memberships.Any(sm => sm.StudentId == studentId));
    }

    public DbResourceGroup GetResourceGroupAssignments(Guid resourceGroupId)
    {
        return _context.ResourceGroups.Query()
            .Include(g => g.Memberships)
                .ThenInclude(rm => rm.Resource)
            .Include(g => g.AccessPolicies)
                .ThenInclude(ap => ap.StudentGroup)
                    .ThenInclude(sg => sg.Memberships)
                        .ThenInclude(sm => sm.Student)
            .SingleOrDefault(rg => rg.Guid == resourceGroupId);
    }

    private readonly TenantContext _context;
}
