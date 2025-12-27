using Endpoints.Dto;

namespace Endpoints.Interfaces;

public interface IStudentResourcesService
{
    StudentResourcesResponse.StudentWithResourcesResponse? GetStudentWithResources(int studentId);
}