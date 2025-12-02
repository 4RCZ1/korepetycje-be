using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class DeleteResourceGroupFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteResourceGroup(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var resourceId = RestIo.GetPathGuid(request, "groupId");
            
            service.DeleteResourceGroup(resourceId, role);

            return string.Empty;
        });
    }
}