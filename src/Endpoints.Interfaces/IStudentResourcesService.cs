using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Interfaces;

public interface IStudentResourcesService
{
    StudentResourcesResponse.StudentWithResourcesResponse? GetStudentWithResources(int studentId, TutorRole role);
}