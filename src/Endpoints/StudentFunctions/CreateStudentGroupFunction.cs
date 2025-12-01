using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class CreateStudentGroupFunction
{
    public static async Task<APIGatewayProxyResponse> CreateStudentGroup(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var body = RestIo.ReadBody<StudentGroupDto>(request);
            service.CreateStudentGroup(body, role);
            return string.Empty;
        });
    }
}
