using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.AddressFunctions;

public class DeleteAddressFunction
{
    public static async Task<APIGatewayProxyResponse> DeleteAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateAddressServiceAsync(identity);
            var externalAddressId = RestIo.GetPathParameter(request, "externalAddressId");
            service.DeleteAddress(externalAddressId, role);
            return string.Empty;
        });
    }
}
