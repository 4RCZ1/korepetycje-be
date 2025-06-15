using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.StudentFunctions;

public class GetStudentFunction
{
    public static async Task<APIGatewayProxyResponse> GetStudent(APIGatewayProxyRequest request)
    {
        var service = await ServiceFactory.CreateStudentServiceAsync();
        var studentExternalId = request.PathParameters["studentExternalId"];
        var student = service.GetStudent(studentExternalId);
        return RestIo.OkJson(student);
    }
}
