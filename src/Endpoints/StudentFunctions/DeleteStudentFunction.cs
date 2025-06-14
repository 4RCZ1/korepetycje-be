using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.StudentFunctions;

public class DeleteStudentFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteStudent(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateStudentServiceAsync();
        var externalStudentId = request.PathParameters["studentExternalId"];
        service.DeleteStudent(externalStudentId);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}
