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
            var includeDeletedString = RestIo.GetOptionalQueryParameter("includeDeleted");
            bool.TryParse(includeDeletedString, out var includeDeleted);
            var lessonExternalId = RestIo.GetOptionalQueryParameter(request, "lessonId");
            return service.GetStudents(role, lessonExternalId, includeDeleted); // todo: fix after merge
        });
    }
}
