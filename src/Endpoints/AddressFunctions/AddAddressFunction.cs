using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces;

namespace Endpoints.AddressFunctions;

public class AddAddressFunction
{
    public static async Task<APIGatewayProxyResponse> AddAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var service = await ServiceFactory.CreateAddressServiceAsync();
            var body = RestIo.ReadBody<AddressDto>(request);
            service.AddAddress(body);
            return string.Empty;
        });
    }
}
