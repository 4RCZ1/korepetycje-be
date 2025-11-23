using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IResourceService
{
    IList<ResourceDto> GetResources(TutorRole role);
    ResourceUrlDto GetDownloadUrlForTutor(Guid externalResourceId, TutorRole role);
    ResourceUrlDto BeginUpload(string filename, TutorRole role);
    void DeleteResource(Guid externalResourceId, TutorRole role);
    void CreateResourceGroup(ResourceGroupDto group, TutorRole role);
}
