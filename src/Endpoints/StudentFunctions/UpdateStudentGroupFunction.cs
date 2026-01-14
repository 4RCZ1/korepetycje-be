using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class UpdateStudentGroupFunction
{
    public static async Task<APIGatewayProxyResponse> UpdateStudentGroup(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var groupId = RestIo.GetPathGuid(request, "groupId");
            var body = RestIo.ReadBody<StudentGroupDto>(request);
            service.UpdateStudentGroup(groupId, body, role);
            return string.Empty;
        });
    }
}
