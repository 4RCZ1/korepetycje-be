using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.AddressFunctions;

public class GetAddressFunction
{
    public async Task<APIGatewayProxyResponse> GetAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateAddressServiceAsync();
            var addressExternalId = RestIo.GetPathParameter(request, "externalAddressId");
            return service.GetAddressById(addressExternalId, role);
        });
    }
}
