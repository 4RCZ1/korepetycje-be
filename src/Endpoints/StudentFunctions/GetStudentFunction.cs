using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.StudentFunctions;

public class GetStudentFunction
{
    public async Task<APIGatewayProxyResponse> GetStudent(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateStudentService();
        var studentExternalId = request.PathParameters["studentExternalId"];
        var student = service.GetStudent(studentExternalId);
        return RestIo.OkJson(student);
    }
}
