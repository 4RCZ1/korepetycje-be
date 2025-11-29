using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class GetResourcesFunction
{
    public static async Task<APIGatewayProxyResponse> GetResources(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            if (identity.AsStudent.HasValue)
            {
                return service.GetResourcesAsStudent(identity.RequireStudent());
            }
            var role = identity.RequireTutor();
            return service.GetResources(role);
        });
    }
}
