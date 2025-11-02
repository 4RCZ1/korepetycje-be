using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class GetStudentsFunction
{
    public static async Task<APIGatewayProxyResponse> GetStudents(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var includeDeletedString = RestIo.GetOptionalQueryParameter(request, "includeDeleted");
            bool.TryParse(includeDeletedString, out var includeDeleted);
            var lessonExternalId = RestIo.GetOptionalQueryParameter(request, "lessonId");
            return service.GetStudents(role, lessonExternalId, includeDeleted);
        });
    }
}
