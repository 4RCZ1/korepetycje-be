using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;

[assembly: LambdaSerializer(typeof(CamelCaseLambdaJsonSerializer))]

namespace Endpoints;

public static class RestIo
{
    public static async Task<APIGatewayProxyResponse> HandleRestBoilerplateAsync<T>(
        APIGatewayProxyRequest request, Func<UserIdentity, Task<T>> f)
    {
        return await UnsafeHandleRestBoilerplateAsync(async () =>
        {
            var token = ReadToken(request);
            var authService = await ServiceFactory.CreateAuthenticationService();
            var identity = await authService.AuthenticateAsync(token);
            return await f.Invoke(identity);
        });
    }

    public static async Task<APIGatewayProxyResponse> UnsafeHandleRestBoilerplateAsync<T>(
        Func<Task<T>> f)
    {
        try
        {
            return OkJson(await f.Invoke() ?? throw new ArgumentNullException());
        }
        catch (BadRequestException e)
        {
            Console.WriteLine(e);
            return new APIGatewayProxyResponse
            {
                Body = e.Message,
                StatusCode = 400,
                Headers = Headers("text/plain"),
            };
        }
        catch (AuthException e)
        {
            Console.WriteLine(e);
            return new APIGatewayProxyResponse
            {
                Body = "Forbidden",
                StatusCode = 403,
                Headers = Headers("text/plain"),
            };
        }
    }

    public static T ReadBody<T>(APIGatewayProxyRequest request)
    {
        if (string.IsNullOrEmpty(request.Body))
            throw new BadRequestException(InvalidBodyMessage);

        try
        {
            return JsonSerializer.Deserialize<T>(request.Body, JsonOptions)
                   ?? throw new BadRequestException(InvalidBodyMessage);
        }
        catch (JsonException e)
        {
            Console.WriteLine(e);
            throw new BadRequestException(InvalidBodyMessage);
        }
    }

    private static string ReadToken(APIGatewayProxyRequest request)
    {
        const string prefix = "Bearer ";
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)
            || !authHeader.StartsWith(prefix))
        {
            throw new BadRequestException("Invalid Authorization header.");
        }
        return authHeader[prefix.Length..];
    }

    public static string GetQueryParameter(APIGatewayProxyRequest request, string name)
    {
        return GetOptionalQueryParameter(request, name) ??
               throw new BadRequestException($"Missing query parameter: {name}");
    }

    public static string? GetOptionalQueryParameter(APIGatewayProxyRequest request, string name)
    {
        if (request.QueryStringParameters is null)
            return null;
        if (!request.QueryStringParameters.TryGetValue(name, out var parameter))
            return null;
        return parameter;
    }

    public static string GetPathParameter(APIGatewayProxyRequest request, string name)
    {
        if (!request.PathParameters.TryGetValue(name, out var parameter))
            throw new BadRequestException($"Missing path parameter: {name}");
        return parameter;
    }

    private static APIGatewayProxyResponse OkJson(Object objectToSerialize)
    {
        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(objectToSerialize, JsonOptions),
            StatusCode = 200,
            Headers = Headers("application/json"),
        };
    }

    private static Dictionary<string, string> Headers(string contentType)
    {
        return new Dictionary<string, string>
        {
            { "Access-Control-Allow-Origin", "*" },
            { "Access-Control-Allow-Headers", "Content-Type,Authorization" },
            { "Access-Control-Allow-Methods", "OPTIONS,GET,POST,PUT,DELETE,PATCH" },
            { "Content-Type", contentType },
        };
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private const string InvalidBodyMessage = "Invalid body.";
}
