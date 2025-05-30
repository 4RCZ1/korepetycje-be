using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class GetLessonsFunction
{
    public class RequestBody
    {
        public string startDate { get; set; } = string.Empty;
    }

    public async Task<APIGatewayProxyResponse> GetLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var responseBody = new Dictionary<string, string>
        {
            { "message", "test message" },
        };

        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(responseBody),
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            }
        };
    }
}
