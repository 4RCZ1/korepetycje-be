using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class DeleteAccessPoliciesFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteAccessPolicies(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var body = RestIo.ReadBody<MultiAssignmentDto>(request);
            service.RevokeAccess(body, role);
            return string.Empty;
        });
    }
}
