using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class PlanLessonsFunction
{
    private class RequestBody
    {
        public required DateTime FirstStartTime { get; set; }
        public required DateTime FirstEndTime { get; set; }
        public required DateTime ScheduleEndTime { get; set; }
        public required int PeriodInDays { get; set; }
        public required IList<string> StudentIds { get; set; }
    }

    public Task<APIGatewayProxyResponse> PlanLessons(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var body = RestIo.ReadBody<RequestBody>(request);
            service.PlanLessons(
                body.FirstStartTime,
                body.FirstEndTime,
                body.ScheduleEndTime,
                body.PeriodInDays,
                body.StudentIds);
            return string.Empty;
        });
    }
}
