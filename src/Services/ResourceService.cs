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

    public ResourceUrlDto GetDownloadUrlForTutor(Guid externalResourceId, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var resource = t.ResourceDao.GetResourceByGuid(externalResourceId);
        return new ResourceUrlDto
        {
            Url = GetDownloadUrl(resource),
        };
    }

    private string? GetDownloadUrl(DbResource? resource)
    {
        if (resource == null)
            return null;
        return _fileStorage.GetDownloadUrl(resource.FilePath);
    }

    public ResourceUrlDto BeginUpload(string filename, TutorRole role)
    {
        using var t = _transactor.BeginTransaction();
        var tutor = t.TutorDao.GetTutor();
        var filePath = $"{tutor.ResourcePathPrefix}/{filename}";
        t.ResourceDao.SaveSingleResource(filePath, $"(single) {filename}");
        t.Commit();
        return new ResourceUrlDto
        {
            Url = _fileStorage.GetUploadUrl(filePath),
        };
    }

    private readonly IFileStorageClient _fileStorage;
    private readonly ITransactor _transactor;
}
