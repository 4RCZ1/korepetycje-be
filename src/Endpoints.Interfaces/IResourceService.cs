using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IResourceService
{
    IList<ResourceDto> GetResourcesAsTutor(TutorRole role);
    ResourceUrlDto GetDownloadUrlForTutor(Guid externalResourceId, TutorRole role);
    ResourceUrlDto BeginUpload(string filename, TutorRole role);
    void DeleteResource(Guid externalResourceId, TutorRole role);
    void DeleteResourceGroup(Guid groupId, TutorRole role);
    IList<ResourceGroupDto> GetResourceGroups(TutorRole role);
    void CreateResourceGroup(ResourceGroupDto group, TutorRole role);
    IList<ResourceDto> GetResourcesAsStudent(StudentRole role);
    void GrantAccess(MultiAssignmentDto dto, TutorRole role);
    void RevokeAccess(MultiAssignmentDto dto, TutorRole role);
    void UpdateResourceGroup(Guid groupId, ResourceGroupDto newContent, TutorRole role);
}
