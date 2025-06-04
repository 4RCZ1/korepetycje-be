using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Endpoints;

public class GetStudentLessonsFunction
{
    
    public async Task<APIGatewayProxyResponse> GetLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        var lessons = service.GetStudentLessons(
            request.PathParameters["studentId"]);

        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(lessons),
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            },
        };
    }
}