using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class CreateResourceGroupFunction
{
    public static async Task<APIGatewayProxyResponse> CreateResourceGroup(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var body = RestIo.ReadBody<ResourceGroupDto>(request);
            service.CreateResourceGroup(body, role);
            return string.Empty;
        });
    }
}
