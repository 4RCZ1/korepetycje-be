using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Database;

public class ResourceStudentsDao : IResourceStudentsDao
{
    private readonly TenantContext _context;

    public ResourceStudentsDao(TenantContext context)
    {
        _context = context;
    }

    public ResourceStudentStructure GetResourceStudentStructure(int resourceId)
    {
        var allStudents = _context.Students.Query()
            .Include(s => s.Memberships)
                .ThenInclude(sm => sm.Group)
                    .ThenInclude(sg => sg!.AccessPolicies)
                        .ThenInclude(ap => ap.ResourceGroup)
                            .ThenInclude(rg => rg!.Memberships)
            .Where(student => student.Memberships.Any(sm =>
                sm.Group != null 
                && sm.Group.IsSingle
                && sm.Group.AccessPolicies.Any(ap =>
                    ap.ResourceGroup != null 
                    && ap.ResourceGroup.IsSingle
                    && ap.ResourceGroup.Memberships.Any(rm => rm.ResourceId == resourceId))))
            .ToList();
        
        
        var studentGroups = _context.StudentGroups.Query()
            .Include(sg => sg.Memberships)
                .ThenInclude(m => m.Student)
            .Include(sg => sg.AccessPolicies)
                .ThenInclude(ap => ap.ResourceGroup)
                    .ThenInclude(rg => rg!.Memberships)
            .Where(sg => !sg.IsSingle 
                         && sg.AccessPolicies.Any(ap =>
                            ap.ResourceGroup != null 
                            && ap.ResourceGroup.IsSingle
                            && ap.ResourceGroup.Memberships.Any(rm => rm.ResourceId == resourceId)))
            .ToList();
        
        
        var resourceGroups = _context.ResourceGroups.Query()
            .Include(rg => rg.Memberships)
                .ThenInclude(rm => rm.Resource)
            .Include(rg => rg.AccessPolicies)
                .ThenInclude(ap => ap.StudentGroup)
                    .ThenInclude(sg => sg!.Memberships)
                        .ThenInclude(m => m.Student)
            .Where(rg => !rg.IsSingle 
                         && rg.Memberships.Any(m => m.ResourceId == resourceId)).ToList();
        
        
        return new ResourceStudentStructure
        {
            DirectStudents = allStudents.Distinct().ToList(),
            StudentGroups = studentGroups, 
            ResourceGroups = resourceGroups
        };
    }
}