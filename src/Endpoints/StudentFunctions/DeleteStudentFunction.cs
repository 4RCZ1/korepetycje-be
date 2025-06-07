using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces;

namespace Endpoints.StudentFunctions;

public class DeleteStudentFunction
{
    public async Task<APIGatewayProxyResponse> DeleteStudent(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateStudentService();
        var externalStudentId = request.PathParameters["studentExternalId"];
        service.DeleteStudent(externalStudentId);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}