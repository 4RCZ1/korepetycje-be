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
        var memberships = group.Resources.Select(r =>
        {
            var resource = t.ResourceDao.GetResourceByGuid(Guid.Parse(r.Id));
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
        t.ResourceDao.SaveResourceGroup(new DbResourceGroup
        {
            IsSingle = false,
            Name = group.Name,
            Memberships = memberships
        });
        t.Commit();
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
    
    
    private readonly IFileStorageClient _fileStorage;
    private readonly ITransactor _transactor;
}
