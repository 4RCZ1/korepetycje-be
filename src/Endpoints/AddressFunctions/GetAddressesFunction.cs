using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.AddressFunctions;

public class GetAddressesFunction
{
    public async Task<APIGatewayProxyResponse> GetAddresses(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateAddressServiceAsync();
            var addresses = service.GetAddresses(role);
            return addresses;
        });
    }
}
