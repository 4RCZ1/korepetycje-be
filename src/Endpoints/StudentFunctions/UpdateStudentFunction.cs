using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces;

namespace Endpoints.StudentFunctions;

public class UpdateStudentFunction
{
    public static async Task<APIGatewayProxyResponse> UpdateStudent(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateStudentServiceAsync();
        if (request.Body is null)
        {
            throw new NullReferenceException("Request body is null");
        }
        var externalStudentId = request.PathParameters["studentExternalId"];
        var student = JsonSerializer.Deserialize<StudentDto>(request.Body);
        if (student != null)
            service.UpdateStudent(externalStudentId, student);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}
