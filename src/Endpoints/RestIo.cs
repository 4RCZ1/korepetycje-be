using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Endpoints.Interfaces;

[assembly: LambdaSerializer(typeof(CamelCaseLambdaJsonSerializer))]

namespace Endpoints;

public static class RestIo
{
    private static readonly Dictionary<string, string> CorsHeaders = new()
    {
        { "Access-Control-Allow-Origin", "*" },
        { "Access-Control-Allow-Headers", "Content-Type,Authorization" },
        { "Access-Control-Allow-Methods", "OPTIONS,GET,POST,PUT,DELETE,PATCH" }
    };
    
    public static async Task<APIGatewayProxyResponse> HandleRestExceptionsAsync<T>(Func<Task<T>> f)
    {
        try
        {
            return OkJson(await f.Invoke() ?? throw new ArgumentNullException());
        }
        catch (BadRequestException e)
        {
            Console.WriteLine(e);
            var headers = new Dictionary<string, string>(CorsHeaders)
            {
                { "Content-Type", "text/plain" }
            };
            return new APIGatewayProxyResponse
            {
                Body = e.Message,
                StatusCode = 400,
                Headers = headers,
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

    public static string GetQueryParameter(APIGatewayProxyRequest request, string name)
    {
        if (!request.QueryStringParameters.TryGetValue(name, out var parameter))
            throw new BadRequestException($"Missing query parameter: {name}");
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
        var headers = new Dictionary<string, string>(CorsHeaders)
        {
            { "Content-Type", "application/json" }
        };
        
        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(objectToSerialize, JsonOptions),
            StatusCode = 200,
            Headers = headers,
        };
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private const string InvalidBodyMessage = "Invalid body.";
}
