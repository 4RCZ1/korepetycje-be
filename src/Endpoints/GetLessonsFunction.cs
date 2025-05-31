using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Database;
using Timetable;
using Timetable.Interfaces;

namespace Endpoints;

public class GetLessonsFunction
{
    public APIGatewayProxyResponse GetLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var lessons = _service.GetLessons(
            request.QueryStringParameters["start-date"],
            request.QueryStringParameters["end-date"]);

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

    private readonly ITimetableService _service = new TimetableService(new LessonDao());
}
