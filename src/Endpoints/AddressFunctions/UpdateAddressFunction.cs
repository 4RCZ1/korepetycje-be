using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.AddressFunctions;

public class UpdateAddressFunction
{
    public static async Task<APIGatewayProxyResponse> UpdateAddress(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateAddressServiceAsync();
            if (request.Body is null)
            {
                throw new NullReferenceException("Request body is null");
            }

            var externalAddressId = RestIo.GetPathParameter(request, "externalAddressId");
            var address = RestIo.ReadBody<AddressDto>(request);
            service.UpdateAddress(externalAddressId, address, role);
            return string.Empty;
        });
    }

}
