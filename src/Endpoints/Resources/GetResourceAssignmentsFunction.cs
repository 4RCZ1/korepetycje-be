using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class GetResourceAssignmentsFunction
{
    public static async Task<APIGatewayProxyResponse> GetResourceAssignments(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var resourceGuid = RestIo.GetPathGuid(request, "resourceGuid");
            return service.GetResourceAssignments(resourceGuid, role);
        });
    }
}