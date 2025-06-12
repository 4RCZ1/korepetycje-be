using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class PlanLessonsFunction
{
    private class RequestBody
    {
        public required DateTime FirstStartTime { get; set; }
        public required DateTime FirstEndTime { get; set; }
        public required int LessonCount { get; set; }
        public required int PeriodInDays { get; set; }
        public required IList<string> StudentIds { get; set; }
    }

    public Task<APIGatewayProxyResponse> PlanLessons(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        return RestIo.HandleRestExceptions(async () =>
        {
            var service = await ServiceFactory.CreateTimetableService();
            var body = RestIo.ReadBody<RequestBody>(request);
            service.PlanLessons(
                body.FirstStartTime,
                body.FirstEndTime,
                body.LessonCount,
                body.PeriodInDays,
                body.StudentIds);
            return string.Empty;
        });
    }
}
