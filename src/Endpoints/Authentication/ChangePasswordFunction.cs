using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.Authentication;

public class ChangePasswordFunction
{
    private class ChangePasswordRequestBody
    {
        public required string AuthSession { get; set; }
        public required string Username { get; set; }
        public required string NewPassword { get; set; }
    }

    public static Task<APIGatewayProxyResponse> ChangePassword(APIGatewayProxyRequest request)
    {
        return RestIo.UnsafeHandleRestBoilerplateAsync(async () =>
        {
            var body = RestIo.ReadBody<ChangePasswordRequestBody>(request);
            var service = await ServiceFactory.CreateAuthenticationService();
            return await service.ChangePasswordAsync(
                body.AuthSession, body.Username, body.NewPassword);
        });
    }
}
