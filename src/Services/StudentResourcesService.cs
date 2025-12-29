using Database;
using Database.Entities;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using Services.Interfaces;

namespace Services;

public class StudentResourcesService : IStudentResourcesService
{

    public StudentResourcesService(ITransactor transactor)
    {
        _transactor = transactor;
    }

    public StudentResourcesResponse.StudentWithResourcesResponse? GetStudentWithResources(int studentId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var student = t.StudentDao.GetStudent(studentId);
        if (student == null) return null;
        var structure = t.StudentResourcesDao.GetStudentResourceStructure(student.Id);
        return new StudentResourcesResponse.StudentWithResourcesResponse
        {
            StudentId = student.Id, Name = student.Name, Surname = student.Surname,
            DirectResources = MapToDto(structure.DirectResources), 
            ResourceGroups = MapToDto(structure.ResourceGroups),
            StudentGroups = MapToDto(structure.StudentGroups)
        };
    }

    private List<ResourceDto> MapToDto(List<DbResource> resources)
    {
        return resources.Select(r => new ResourceDto
        {
            Id = r.Guid.ToString(), 
            Name = r.Filename 
        }).ToList();
    }

    private List<StudentResourcesResponse.ResourceGroupWithResourcesDto> MapToDto(
        List<DbResourceGroup> resourceGroups)
    {
        return resourceGroups.Select(rg => new StudentResourcesResponse.ResourceGroupWithResourcesDto
        {
            Id = rg.Guid.ToString(),
            Name = rg.Name, 
            IsSingle = rg.IsSingle,
            Resources = MapToDto(rg.Memberships?.Where(m => m.Resource != null).Select(m => m.Resource!).ToList() ??
                                 new List<DbResource>())
        }).ToList();
    }

    private List<StudentResourcesResponse.StudentGroupWithResourcesDto> MapToDto(List<DbStudentGroup> studentGroups)
    {
        return studentGroups.Select(sg => new StudentResourcesResponse.StudentGroupWithResourcesDto
        {
            Id = sg.Guid.ToString(), 
            Name = sg.Name,
            IsSingle = sg.IsSingle,
            DirectResources = MapToDto(sg.AccessPolicies?
                .Where(ap => ap.ResourceGroup != null && ap.ResourceGroup.IsSingle)
                .SelectMany(ap => ap.ResourceGroup!.Memberships)
                .Where(m => m.Resource != null)
                .Select(m => m.Resource!).Distinct()
                .ToList() ?? new List<DbResource>()),
            ResourceGroups =
                MapToDto(sg.AccessPolicies?
                    .Where(ap => ap.ResourceGroup != null && !ap.ResourceGroup.IsSingle)
                    .Select(ap => ap.ResourceGroup!).Distinct().ToList() ?? new List<DbResourceGroup>())
        }).ToList();
    }
    private readonly ITransactor _transactor;
}