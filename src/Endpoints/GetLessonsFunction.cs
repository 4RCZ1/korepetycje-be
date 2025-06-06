using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Endpoints.Interfaces;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Endpoints;

public class GetLessonsFunction
{
    public async Task<APIGatewayProxyResponse> GetLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        var studentExternalId = request.QueryStringParameters["student-external-id"];
        if (studentExternalId == null)
        {
            return OkJson(service.GetLessons(
                request.QueryStringParameters["start-date"],
                request.QueryStringParameters["end-date"]));
        }
        else
        {
            // todo: handle date ranges
            return OkJson(service.GetStudentLessons(studentExternalId));
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
