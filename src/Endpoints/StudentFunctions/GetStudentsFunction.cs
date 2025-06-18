using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.StudentFunctions;

public class GetStudentsFunction
{
    public async Task<APIGatewayProxyResponse> GetStudents(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateStudentServiceAsync();
            if (request.QueryStringParameters.TryGetValue("lessonId", out var lessonExternalId))
                return service.GetStudents(lessonExternalId);
            return service.GetStudents();
        });
    }
}
