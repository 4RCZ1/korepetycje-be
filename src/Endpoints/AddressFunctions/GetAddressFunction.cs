using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.AddressFunctions;

public class GetAddressFunction
{
    public async Task<APIGatewayProxyResponse> GetAddress(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateAddressService();
        var addressExternalId = request.PathParameters["addressExternalId"];
        var address = service.GetAddressById(addressExternalId);
        return Response.OkJson(address);
    }
}