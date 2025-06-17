using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.AddressFunctions;

public class DeleteAddressFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateAddressServiceAsync();
            var externalAddressId = RestIo.GetPathParameter(request, "externalAddressId");
            service.DeleteAddress(externalAddressId);
            return string.Empty;
        });
    }
}