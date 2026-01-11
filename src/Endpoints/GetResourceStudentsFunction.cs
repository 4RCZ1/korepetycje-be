using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class GetResourceStudentsFunction
{
    public static async Task<APIGatewayProxyResponse> GetResourceStudents(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceStudentsServiceAsync(identity);
            var resourceGuid = RestIo.GetPathGuid(request, "resourceGuid");
            return service.GetResourceWithStudents(resourceGuid, role);
        });
    }
}