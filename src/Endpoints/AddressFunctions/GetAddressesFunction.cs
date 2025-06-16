using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.AddressFunctions;

public class GetAddressesFunction
{
    public async Task<APIGatewayProxyResponse> GetAddresses(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateAddressServiceAsync();
            var addresses = service.GetAddresses();
            return addresses;
        });
    }
}