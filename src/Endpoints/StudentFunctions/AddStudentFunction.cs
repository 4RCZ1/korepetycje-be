using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;

namespace Endpoints.StudentFunctions;

public class AddStudentFunction
{
    public static async Task<APIGatewayProxyResponse> AddStudent(APIGatewayProxyRequest request)
    {
        var service = await ServiceFactory.CreateStudentServiceAsync();
        var body = RestIo.ReadBody<StudentDto>(request);
        service.AddStudent(body);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }

}
