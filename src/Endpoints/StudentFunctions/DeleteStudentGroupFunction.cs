using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class DeleteStudentGroupFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteStudentGroup(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);
            var groupId = RestIo.GetPathGuid(request, "groupId");

            service.DeleteStudentGroup(groupId, role);

            return string.Empty;
        });
    }
}
