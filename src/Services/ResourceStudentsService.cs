using Database.Entities;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using Services.Interfaces;

namespace Services;

public class ResourceStudentsService : IResourceStudentsService
{
    public ResourceStudentsService(ITransactor transactor)
    {
        _transactor = transactor;
    }
    
    
    public ResourceStudentsResponse.ResourceWithStudentsResponse? GetResourceWithStudents(Guid externalResourceId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var resource = t.ResourceDao.GetResourceByGuid(externalResourceId);
        if (resource == null) return null;
        var structure = t.ResourceStudentsDao.GetResourceStudentStructure(resource.Id);
        return new ResourceStudentsResponse.ResourceWithStudentsResponse
        {
            Id = resource.Guid.ToString(),
            Name = resource.Filename, 
            DirectStudents = MapToDto(structure.DirectStudents),
            StudentGroups = MapToStudentGroupDto(structure.StudentGroups),
            ResourceGroups = MapToDto(structure.ResourceGroups)
        };
    }

    private List<StudentDto> MapToDto(List<DbStudent> students)
    {
        return students.Select(s => new StudentDto
            { 
                ExternalId = s.Id.ToString(),
                Name = s.Name,
                Surname = s.Surname,
                PhoneNumber = s.PhoneNumber,
                IsDeleted = s.IsDeleted,
            }
        ).ToList();
    }

    private List<ResourceStudentsResponse.StudentGroupWithStudentsDto> MapToStudentGroupDto(List<DbStudentGroup> studentGroups)
    {
        return studentGroups.Select(sg => new ResourceStudentsResponse.StudentGroupWithStudentsDto
        {
            Id = sg.Guid.ToString(), 
            Name = sg.Name, 
            IsSingle = sg.IsSingle,
            Students = MapToDto(sg.Memberships?
                                    .Where(m => m.Student != null)
                                    .Select(m => m.Student!)
                                    .ToList() ??
                                new List<DbStudent>())
        }).ToList();
    }

    private List<ResourceStudentsResponse.ResourceGroupWithStudentsDto> MapToDto(List<DbResourceGroup> resourceGroups)
    {
        return resourceGroups.Select(rg => new ResourceStudentsResponse.ResourceGroupWithStudentsDto
        {
            Id = rg.Guid.ToString(), 
            Name = rg.Name, 
            IsSingle = rg.IsSingle,
            DirectStudents = MapToDto(rg.AccessPolicies
                ?.Where(ap => ap.StudentGroup != null 
                              && ap.StudentGroup.IsSingle)
                .SelectMany(ap => ap.StudentGroup!.Memberships)
                .Where(m => m.Student != null)
                .Select(m => m.Student!)
                .Distinct()
                .ToList() ?? new List<DbStudent>()),
            StudentGroups =
                MapToStudentGroupDto(
                    rg.AccessPolicies?
                        .Where(ap => ap.StudentGroup != null 
                                     && !ap.StudentGroup.IsSingle)
                        .Select(ap => ap.StudentGroup!)
                        .Distinct()
                        .ToList() ?? new List<DbStudentGroup>())
        }).ToList();
    }
    
    private readonly ITransactor _transactor;
}