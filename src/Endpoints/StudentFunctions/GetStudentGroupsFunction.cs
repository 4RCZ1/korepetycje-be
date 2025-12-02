using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.StudentFunctions;

public class GetStudentGroupsFunction
{
    public static async Task<APIGatewayProxyResponse> GetStudentGroups(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateStudentServiceAsync(identity);

            return service.GetStudentGroups(role);
        });
    }
}