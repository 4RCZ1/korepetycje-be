using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class GetStudentLessonsFunction
{
    // todo: require date ranges
    public async Task<APIGatewayProxyResponse> GetStudentLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        var studentExternalId = request.PathParameters["studentExternalId"];
        var lessons = service.GetStudentLessons(studentExternalId);

        return new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(lessons),
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            }
        };
    }
}
