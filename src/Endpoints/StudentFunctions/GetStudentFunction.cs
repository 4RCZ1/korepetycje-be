using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class GetStudentFunction
{
    public static async Task<APIGatewayProxyResponse> GetStudent(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync();
            var studentExternalId = RestIo.GetPathParameter(request, "studentExternalId");
            var includeDeletedString = RestIo.GetOptionalQueryParameter(request, "includeDeleted");
            bool.TryParse(includeDeletedString, out var includeDeleted);
            return service.GetStudent(studentExternalId, role, includeDeleted);
        });
    }
}
