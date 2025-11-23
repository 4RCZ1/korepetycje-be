using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IResourceService
{
    IList<ResourceDto> GetResources(TutorRole role);
    ResourceUrlDto GetDownloadUrlForTutor(Guid externalResourceId, TutorRole role);
    ResourceUrlDto BeginUpload(string filename, TutorRole role);
    void DeleteResourceForTutor(Guid externalResourceId, TutorRole role);
}
