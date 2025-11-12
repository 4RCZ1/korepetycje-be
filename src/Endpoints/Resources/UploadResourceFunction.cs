using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class UploadResourceFunction
{
    private class UploadResourceRequestBody
    {
        public required string Filename { get; set; }
    }

    public static async Task<APIGatewayProxyResponse> UploadResource(APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var body = RestIo.ReadBody<UploadResourceRequestBody>(request);
            return service.BeginUpload(body.Filename, role);
        });
    }
}
