using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.StudentFunctions;

public class GetStudentFunction
{
    public static async Task<APIGatewayProxyResponse> GetStudent(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(async () =>
        {
            var service = await ServiceFactory.CreateStudentServiceAsync();
            var studentExternalId = RestIo.GetPathParameter(request, "studentExternalId");
            var student = service.GetStudent(studentExternalId);
            return student;
        });
    }
}
