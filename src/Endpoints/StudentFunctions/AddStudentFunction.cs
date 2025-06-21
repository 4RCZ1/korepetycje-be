using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;

namespace Endpoints.StudentFunctions;

public class AddStudentFunction
{
    public static async Task<APIGatewayProxyResponse> AddStudent(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(async () =>
        {
            var service = await ServiceFactory.CreateStudentServiceAsync();
            var body = RestIo.ReadBody<StudentDto>(request);
            service.AddStudent(body);
            return string.Empty;
        });
    }

}
