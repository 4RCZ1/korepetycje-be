using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Interfaces;

namespace Endpoints.AddressFunctions;

public class UpdateAddressFunction
{
    public static async Task<APIGatewayProxyResponse> UpdateAddress(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateAddressServiceAsync();
        if (request.Body is null)
        {
            throw new NullReferenceException("Request body is null");
        }
        var externalAddressId = RestIo.GetPathParameter(request, "externalAddressId");
        var address = RestIo.ReadBody<AddressDto>(request);
        service.UpdateAddress(externalAddressId, address);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }

}