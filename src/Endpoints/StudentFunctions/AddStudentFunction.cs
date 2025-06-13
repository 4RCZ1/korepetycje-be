using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces;

namespace Endpoints.StudentFunctions;

public class AddStudentFunction
{
    public async Task<APIGatewayProxyResponse> AddStudent(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateStudentServiceAsync();
        if (request.Body is null)
        {
            throw new NullReferenceException("Request body is null");
        }
        var body = JsonSerializer.Deserialize<StudentDto>(request.Body);
        if (body != null) service.AddStudent(body);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
    
}