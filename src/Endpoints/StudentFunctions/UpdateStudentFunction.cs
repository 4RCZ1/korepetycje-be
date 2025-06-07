using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces;

namespace Endpoints.StudentFunctions;

public class UpdateStudentFunction
{
    public async Task<APIGatewayProxyResponse> UpdateStudent(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateStudentService();
        if (request.Body is null)
        {
            throw new NullReferenceException("Request body is null");
        }
        var externalStudentId = request.PathParameters["studentExternalId"];
        var body = JsonSerializer.Deserialize<StudentDto>(request.Body);
        service.UpdateStudent(externalStudentId, body);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}