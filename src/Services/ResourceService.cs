using Database.Entities;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;
using Services.Interfaces;

namespace Services;

public class ResourceService : IResourceService
{
    public ResourceService(ITransactor transactor, IFileStorageClient fileStorage)
    {
        _transactor = transactor;
        _fileStorage = fileStorage;
    }

    public IList<ResourceDto> GetResourcesAsTutor(TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        return t.ResourceDao.GetAllResources().Select(r => new ResourceDto
        {
            Id = r.Guid.ToString(),
            Name = r.Filename,
        }).ToList();
    }

    public ResourceUrlDto GetDownloadUrlForTutor(Guid externalResourceId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var resource = t.ResourceDao.GetResourceByGuid(externalResourceId);
        return new ResourceUrlDto
        {
            Url = GetDownloadUrl(resource, t),
        };
    }

    public ResourceUrlDto GetDownloadUrlForStudent(Guid externalResourceId, StudentRole role)
    {
        using var t = _transactor.BeginTransaction();
        var studentId = int.Parse(role.ExternalStudentId);
        if (!t.ResourceDao.StudentHasAccessToResource(studentId, externalResourceId))
            throw new BadRequestException("Nie posiadasz dostępu do tego zasobu.");
        var resource = t.ResourceDao.GetResourceByGuid(externalResourceId);
        return new ResourceUrlDto
        {
            Url = GetDownloadUrl(resource, t),
        };
    }

    private string? GetDownloadUrl(DbResource? resource, ITransaction t)
    {
        if (resource == null)
            return null;
        return _fileStorage.GetDownloadUrl(GetFilePath(resource.Filename, t));
    }

    public ResourceUrlDto BeginUpload(string filename, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var url = _fileStorage.GetUploadUrl(GetFilePath(filename, t));
        t.ResourceDao.SaveSingleResource(filename, $"(single) {filename}");
        t.Commit();
        return new ResourceUrlDto { Url = url };
    }

    private static string GetFilePath(string filename, ITransaction t)
    {
        var tutor = t.TutorDao.GetTutor();
        return $"{tutor.ResourcePathPrefix}/{filename}";
    }

    public void DeleteResource(Guid externalResourceId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();

        var resource = t.ResourceDao.GetResourceByGuid(externalResourceId);

        if (resource == null)
            return;

        t.ResourceDao.DeleteResource(resource);

        var resourceGroup = t.ResourceDao.GetResourceSingleGroupByResourceId(resource.Id);
        t.ResourceDao.DeleteGroupByGuid(resourceGroup.Guid);

        t.Commit();

        var filePath = GetFilePath(resource.Filename, t);
        _fileStorage.DeleteFile(filePath);
    }

    public void DeleteResourceGroup(Guid groupId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        t.ResourceDao.DeleteGroupByGuid(groupId);
        t.Commit();
    }

    public void CreateResourceGroup(ResourceGroupDto group, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var memberships = CreateMemberships(t.ResourceDao, group);
        if (group.Name == null)
            throw new BadRequestException("Nazwa grupy musi zostać podana.");
        t.ResourceDao.SaveResourceGroup(new DbResourceGroup
        {
            IsSingle = false,
            Name = group.Name,
            Memberships = memberships
        });
        t.Commit();
    }

    private static List<DbResourceMembership> CreateMemberships(IResourceDao dao, ResourceGroupDto group)
    {
        return group.Resources.Select(r =>
        {
            var resource = dao.GetResourceByGuid(Guid.Parse(r.Id));
            if (resource == null)
            {
                throw new BadRequestException(
                    "Nie znaleziono jednego z podanych zasobów."
                    + " Prawdopodobnie został usunięty. Odśwież stronę i spróbuj ponownie.");
            }
            return new DbResourceMembership
            {
                ResourceId = resource.Id,
            };
        }).ToList();
    }

    public void GrantAccess(MultiAssignmentDto dto, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var studentGroups = GetStudentGroups(dto, t.StudentDao);
        var resourceGroups = GetResourceGroups(dto, t.ResourceDao);
        foreach (var studentGroup in studentGroups)
        {
            foreach (var resourceGroup in resourceGroups)
            {
                t.ResourceDao.SaveAccessPolicyIfNotExists(studentGroup.Id, resourceGroup.Id);
            }
        }

        t.Commit();
    }

    public void RevokeAccess(MultiAssignmentDto dto, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var studentGroups = GetStudentGroups(dto, t.StudentDao);
        var resourceGroups = GetResourceGroups(dto, t.ResourceDao);
        foreach (var studentGroup in studentGroups)
        {
            foreach (var resourceGroup in resourceGroups)
            {
                t.ResourceDao.DeleteAccessPolicy(studentGroup.Id, resourceGroup.Id);
            }
        }
        t.Commit();
    }

    private static List<DbStudentGroup> GetStudentGroups(MultiAssignmentDto dto, IStudentDao dao)
    {
        var studentGroups = new List<DbStudentGroup>();
        foreach (var externalStudentId in dto.StudentIds ?? [])
        {
            var student = dao.GetStudent(int.Parse(externalStudentId))
                          ?? throw CreateMissingEntityException("uczniów");
            studentGroups.Add(dao.GetStudentSingleGroupByStudentId(student.Id));
        }

        foreach (var studentGroupGuid in dto.StudentGroupIds ?? [])
        {
            var studentGroup = dao.GetStudentGroupByGuid(studentGroupGuid)
                               ?? throw CreateMissingEntityException("grup uczniów");
            studentGroups.Add(studentGroup);
        }

        return studentGroups;
    }

    private static List<DbResourceGroup> GetResourceGroups(MultiAssignmentDto dto, IResourceDao dao)
    {
        var resourceGroups = new List<DbResourceGroup>();
        foreach (var resourceGuid in dto.ResourceIds ?? [])
        {
            var resource = dao.GetResourceByGuid(resourceGuid)
                           ?? throw CreateMissingEntityException("zasobów");
            resourceGroups.Add(dao.GetResourceSingleGroupByResourceId(resource.Id));
        }

        foreach (var resourceGroupGuid in dto.ResourceGroupIds ?? [])
        {
            var resourceGroup = dao.GetResourceGroupByGuid(resourceGroupGuid)
                                ?? throw CreateMissingEntityException("grup zasobów");
            resourceGroups.Add(resourceGroup);
        }

        return resourceGroups;
    }

    private static BadRequestException CreateMissingEntityException(string entityType)
    {
        return new BadRequestException($"Nie znaleziono niektórych podanych {entityType}.");
    }

    public IList<ResourceDto> GetResourcesAsStudent(StudentRole role)
    {
        using var t = _transactor.BeginTransaction();

        var studentId = int.Parse(role.ExternalStudentId);

        return t.ResourceDao.GetStudentResources(studentId)
            .Select(r => new ResourceDto()
            {
                Id = r.Guid.ToString(),
                Name = r.Filename
            })
            .ToList();
    }

    public IList<ResourceGroupDto> GetResourceGroups(TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        return t.ResourceDao.GetAllResourceGroups().Select(r => new ResourceGroupDto
        {
            Id = r.Guid.ToString(),
            Name = r.Name,
            Resources = r.Memberships.Select(m => new ResourceDto()
            {
                Id = m.Resource?.Guid.ToString() ?? String.Empty,
                Name = m.Resource?.Filename ?? String.Empty,
            }).ToList()
        }).ToList();
    }

    public ResourceAssignmentsDto GetResourceAssignments(Guid resourceId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var assignedStudents = t.ResourceDao.GetResourceAssignments(resourceId);
        return new ResourceAssignmentsDto()
        {
            resourdeGuid = resourceId.ToString(),
            students = assignedStudents.Select(s => new StudentDto()
            {
                ExternalId = s.Id.ToString(),
                Name = s.Name,
                Surname = s.Surname,
            }).ToList()
        };
    }

    public void UpdateResourceGroup(Guid groupId, ResourceGroupDto newContent, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var group = t.ResourceDao.GetResourceGroupByGuid(groupId);
        if (group == null)
            throw new BadRequestException("Aktualizowana grupa została już usunięta.");
        t.ResourceDao.EmptyResourceGroup(group.Id);
        group.Memberships = CreateMemberships(t.ResourceDao, newContent);
        if (newContent.Name != null)
            group.Name = newContent.Name;
        t.ResourceDao.SaveResourceGroup(group);
        t.Commit();
    }

    public ResourceGroupAssignmentsDto GetResourceGroupAssignments(Guid resourceGroupId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var group = t.ResourceDao.GetResourceGroupByGuid(resourceGroupId);
        if (group == null)
            throw new BadRequestException("Grupa nie istnieje.");
        var response = t.ResourceDao.GetResourceGroupAssignments(resourceGroupId);

        var directStudents = response.AccessPolicies
            .Select(ac => ac.StudentGroup)
                .Where(g => g != null && g.IsSingle)
            .SelectMany(g => g!.Memberships)
                .Where(m => m.Student != null)
                .Select(m => m.Student!);

        var resources = response.Memberships
            .Where(m => m.Resource != null)
            .Select(m => m.Resource!);

        var studentGroups = response.AccessPolicies
            .Where(ap => ap.StudentGroup != null && !ap.StudentGroup.IsSingle)
            .Select(ac => ac.StudentGroup!);
            

        return new ResourceGroupAssignmentsDto()
        {
            Id = response.Guid.ToString(),
            Name = response.Name,
            DirectStudents = directStudents.Select(s => new StudentDto()
            {
                ExternalId = s.Id.ToString(),
                Name = s.Name,
                Surname = s.Surname,
                PhoneNumber = s.PhoneNumber,
                IsDeleted = s.IsDeleted,
            }).ToList(),
            Resources = resources.Select(r => new ResourceDto()
            {
                Id = r.Guid.ToString(),
                Name = r.Filename
            }).ToList(),
            StudentGroups = studentGroups.Select(s => new StudentGroupDto()
            {
                Id = s.Guid.ToString(),
                Name = s.Name
            }).ToList()
        };
    }

    private readonly IFileStorageClient _fileStorage;
    private readonly ITransactor _transactor;
}
