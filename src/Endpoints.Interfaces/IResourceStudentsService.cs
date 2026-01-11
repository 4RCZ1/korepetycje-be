using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IResourceStudentsService
{
    ResourceStudentsResponse.ResourceWithStudentsResponse? GetResourceWithStudents(Guid externalResourceId, TutorRole role);
}