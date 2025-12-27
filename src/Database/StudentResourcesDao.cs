using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Database;

public class StudentResourcesDao : IStudentResourcesDao
{
    private readonly TenantContext _context;

    public StudentResourcesDao(TenantContext context)
    {
        _context = context;
    }

    public StudentResourceStructure GetStudentResourceStructure(int studentId)
    {
        var allResources = _context.Resources.Query()
            .Include(r => r.Memberships)
                .ThenInclude(rm => rm.Group)
                    .ThenInclude(rg => rg!.AccessPolicies)
                        .ThenInclude(ap => ap.StudentGroup)
                            .ThenInclude(sg => sg!.Memberships)
             .Where(resource => resource.Memberships
                .Any(rm => rm.Group != null 
                           && rm.Group.AccessPolicies.Any(ap => ap.StudentGroup != null 
                                                                && ap.StudentGroup.IsSingle == true
                           && ap.StudentGroup.Memberships.Any(sm => sm.StudentId == studentId))))
            .ToList();
        
        var resourceGroups = _context.ResourceGroups.Query()
            .Include(rg => rg.Memberships)
                .ThenInclude(rm => rm.Resource)
            .Include(rg => rg.AccessPolicies)
                .ThenInclude(ap => ap.StudentGroup)
                    .ThenInclude(sg => sg!.Memberships)
            .Where(rg => !rg.IsSingle && rg.AccessPolicies
                .Any(ap => ap.StudentGroup != null 
                           && ap.StudentGroup.IsSingle == true
                           && ap.StudentGroup.Memberships.Any(sm => sm.StudentId == studentId)))
            .ToList();
        
        var studentGroups = _context.StudentGroups.Query()
            .Include(g => g.Memberships)
                .ThenInclude(m => m.Student)
            .Include(g => g.AccessPolicies)
                .ThenInclude(ap => ap.ResourceGroup)
                    .ThenInclude(rg => rg!.Memberships)
                        .ThenInclude(rm => rm.Resource)
            .Where(g => !g.IsSingle 
                        && g.Memberships.Any(m => m.StudentId == studentId))
            .ToList();
        
        
        return new StudentResourceStructure
        {
            DirectResources = allResources.Where(r => r.Memberships
                    .Any(rm => rm.Group?.IsSingle == true)).Distinct()
                    .ToList(),
            ResourceGroups = resourceGroups, 
            StudentGroups = studentGroups
        };
    }
}
