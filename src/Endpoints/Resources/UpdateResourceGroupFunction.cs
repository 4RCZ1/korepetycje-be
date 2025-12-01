using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class UpdateResourceGroupFunction
{
    public static async Task<APIGatewayProxyResponse> UpdateResourceGroup(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var groupId = RestIo.GetPathGuid(request, "groupId");
            var body = RestIo.ReadBody<ResourceGroupDto>(request);
            service.UpdateResourceGroup(groupId, body, role);
            return string.Empty;
        });
    }
}
