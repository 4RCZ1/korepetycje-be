using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;

namespace Endpoints.StudentFunctions;

public class UpdateStudentFunction
{
    public static async Task<APIGatewayProxyResponse> UpdateStudent(APIGatewayProxyRequest request)
    {
        var service = await ServiceFactory.CreateStudentServiceAsync();
        var externalStudentId = RestIo.GetPathParameter(request, "studentExternalId");
        var student = RestIo.ReadBody<StudentDto>(request);
        service.UpdateStudent(externalStudentId, student);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}
