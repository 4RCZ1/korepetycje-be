using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class DeleteResourceFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteResource(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var resourceId = RestIo.GetPathGuid(request, "resourceGuid");
    
            service.DeleteResource(resourceId, role);
    
            return string.Empty;
        });


    }
}