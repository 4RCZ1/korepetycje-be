using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.AddressFunctions;

public class GetAddressFunction
{
    public async Task<APIGatewayProxyResponse> GetAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateAddressServiceAsync();
            var addressExternalId = RestIo.GetPathParameter(request, "externalAddressId");
            var address = service.GetAddressById(addressExternalId);
            return RestIo.OkJson(address);
        });
    }
}