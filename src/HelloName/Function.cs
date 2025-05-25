using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloName;

public class Function
{
    public class RequestBody
    {
        public string name { get; set; } = string.Empty;
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
    {
        try
        {
            // Parse the JSON body
            var requestBody = JsonSerializer.Deserialize<RequestBody>(apigProxyEvent.Body ?? "{}");
            
            // Get the name parameter or use a default
            var name = !string.IsNullOrEmpty(requestBody?.name) ? requestBody.name : "World";
            
            // Create response body
            var responseBody = new Dictionary<string, string>
            {
                { "message", $"hello {name}" }
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(responseBody),
                StatusCode = 200,
                Headers = new Dictionary<string, string> 
                { 
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "Content-Type" },
                    { "Access-Control-Allow-Methods", "POST, OPTIONS" }
                }
            };
        }
        catch (JsonException)
        {
            // Handle invalid JSON
            var errorResponse = new Dictionary<string, string>
            {
                { "error", "Invalid JSON format" }
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(errorResponse),
                StatusCode = 400,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
        catch (System.Exception ex)
        {
            // Handle other errors
            context.Logger.LogLine($"Error: {ex.Message}");
            
            var errorResponse = new Dictionary<string, string>
            {
                { "error", "Internal server error" }
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(errorResponse),
                StatusCode = 500,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}