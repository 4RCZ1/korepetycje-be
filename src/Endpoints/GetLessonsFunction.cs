using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Endpoints.Interfaces;

[assembly: LambdaSerializer(typeof(CamelCaseLambdaJsonSerializer))]

namespace Endpoints;

public class GetLessonsFunction
{
    public async Task<APIGatewayProxyResponse> GetLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        if (request.QueryStringParameters.TryGetValue("studentExternalId", out var studentExternalId))
        {
            return OkJson(service.GetStudentLessons(
                studentExternalId,
                request.QueryStringParameters["startTime"],
                request.QueryStringParameters["endTime"]));
        }
        else
        {
            return OkJson(service.GetLessons(
                request.QueryStringParameters["startTime"],
                request.QueryStringParameters["endTime"]));
        }
    }

    private static APIGatewayProxyResponse OkJson(IList<LessonDto> lessons)
    {
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
