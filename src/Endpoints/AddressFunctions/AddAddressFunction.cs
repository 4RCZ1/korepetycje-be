using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces;

namespace Endpoints.AddressFunctions;

public class AddAddressFunction
{
    public static async Task<APIGatewayProxyResponse> AddAddress(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateAddressServiceAsync();
        var body = RestIo.ReadBody<AddressDto>(request);
        service.AddAddress(body);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}