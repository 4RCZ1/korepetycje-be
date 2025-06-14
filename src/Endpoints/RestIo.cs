using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Endpoints.Interfaces;

[assembly: LambdaSerializer(typeof(CamelCaseLambdaJsonSerializer))]

namespace Endpoints;

public static class RestIo
{
    public static async Task<APIGatewayProxyResponse> HandleRestExceptionsAsync<T>(Func<Task<T>> f)
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
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "text/plain" },
                },
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

    public static APIGatewayProxyResponse OkJson(Object objectToSerialize)
    {
        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(objectToSerialize, JsonOptions),
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            },
        };
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private const string InvalidBodyMessage = "Invalid body.";
}
