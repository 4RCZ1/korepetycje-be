using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IResourceService
{
    ResourceUrlDto GetDownloadUrlForTutor(Guid externalResourceId, TutorRole role);
    ResourceUrlDto BeginUpload(string filename, TutorRole role);
}
