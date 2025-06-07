using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints;

public class Response
{
    public static APIGatewayProxyResponse OkJson(Object objectToSerialize)
    {
        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(objectToSerialize),
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            },
        };
    }
}