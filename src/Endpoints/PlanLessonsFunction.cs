using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class PlanLessonsFunction
{
    private class RequestBody
    {
        public required string BeginTime { get; set; }
        public required string EndDate { get; set; }
        public required int PeriodInDays { get; set; }
        public required string StudentExternalId { get; set; }
        public required int DurationInMinutes { get; set; }
    }

    public async Task<APIGatewayProxyResponse> PlanLessons(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        var body = JsonSerializer.Deserialize<RequestBody>(request.Body);
        service.PlanLessons(
            body.BeginTime,
            body.EndDate,
            body.PeriodInDays,
            body.StudentExternalId,
            body.DurationInMinutes);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}
