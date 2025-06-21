using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.StudentFunctions;

public class GetStudentsFunction
{
    public async Task<APIGatewayProxyResponse> GetStudents(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(async () =>
        {
            var service = await ServiceFactory.CreateStudentServiceAsync();
            var lessonExternalId = RestIo.GetOptionalQueryParameter(request, "lessonId");
            if (lessonExternalId is not null)
                return service.GetStudents(lessonExternalId);
            return service.GetStudents();
        });
    }
}
