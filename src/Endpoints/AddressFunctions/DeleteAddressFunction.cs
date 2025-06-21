using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints.AddressFunctions;

public class DeleteAddressFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(async () =>
        {
            var service = await ServiceFactory.CreateAddressServiceAsync();
            var externalAddressId = RestIo.GetPathParameter(request, "externalAddressId");
            service.DeleteAddress(externalAddressId);
            return string.Empty;
        });
    }
}