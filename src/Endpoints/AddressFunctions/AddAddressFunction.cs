using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.AddressFunctions;

public class AddAddressFunction
{
    public static async Task<APIGatewayProxyResponse> AddAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateAddressServiceAsync(identity);
            var body = RestIo.ReadBody<AddressDto>(request);
            service.AddAddress(body, role);
            return string.Empty;
        });
    }
}
