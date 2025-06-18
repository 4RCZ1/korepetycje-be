using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints.Authentication;

public class LogInFunction
{
    private class LogInRequestBody
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public static Task<APIGatewayProxyResponse> LogIn(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var body = RestIo.ReadBody<LogInRequestBody>(request);
            var service = await ServiceFactory.CreateAuthenticationService();
            return await service.LogInAsync(body.Username, body.Password);
        });
    }
}
