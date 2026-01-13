using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.Resources;

public class GetResourcesGroupAssignmentsFunction
{
    public async Task<APIGatewayProxyResponse> GetResourcesGroupAssignments(APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateResourceServiceAsync(identity);
            var groupGuid = RestIo.GetPathGuid(request, "resourceGroupGuid");
            var result = service.GetResourceGroupAssignments(groupGuid, role);
            return result;
        });
    }
}

