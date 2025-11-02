using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class UpdateStudentFunction
{
    public static async Task<APIGatewayProxyResponse> UpdateStudent(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var externalStudentId = RestIo.GetPathParameter(request, "studentExternalId");
            var student = RestIo.ReadBody<StudentDto>(request);
            service.UpdateStudent(externalStudentId, student, role);
            return string.Empty;
        });
    }
}
