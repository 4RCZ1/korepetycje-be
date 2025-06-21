using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class GetStudentsFunction
{
    public async Task<APIGatewayProxyResponse> GetStudents(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync();
            var lessonExternalId = RestIo.GetOptionalQueryParameter(request, "lessonId");
            if (lessonExternalId is not null)
                return service.GetStudents(role, lessonExternalId);
            return service.GetStudents(role);
        });
    }
}
