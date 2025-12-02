using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class CreateAccessPoliciesFunction
{
    public static async Task<APIGatewayProxyResponse> CreateAccessPolicies(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var body = RestIo.ReadBody<MultiAssignmentDto>(request);
            service.GrantAccess(body, role);
            return string.Empty;
        });
    }
}
