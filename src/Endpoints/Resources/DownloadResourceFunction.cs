using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class DownloadResourceFunction
{
    public static async Task<APIGatewayProxyResponse> DownloadResource(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var resourceGuid = RestIo.GetPathGuid(request, "resourceGuid");
            return service.GetDownloadUrlForTutor(resourceGuid, role);
        });
    }
}
