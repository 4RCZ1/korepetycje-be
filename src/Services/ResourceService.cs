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

    public IList<ResourceDto> GetResources(TutorRole role)
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
        t.ResourceDao.DeleteGroup(resourceGroup);
    
        t.Commit();
        
        var filePath = GetFilePath(resource.Filename, t);
        _fileStorage.DeleteFile(filePath);
    }
    
    
    private readonly IFileStorageClient _fileStorage;
    private readonly ITransactor _transactor;
}
