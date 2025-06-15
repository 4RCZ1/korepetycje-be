using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints;

public class PlanLessonsFunction
{
    private class PlanLessonsRequestBody
    {
        public required DateTime FirstStartTime { get; set; }
        public required DateTime FirstEndTime { get; set; }
        public required DateTime ScheduleEndTime { get; set; }
        public required int PeriodInDays { get; set; }
        public required string AddressId { get; set; }
        public required IList<string> StudentIds { get; set; }
    }

    public static Task<APIGatewayProxyResponse> PlanLessons(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var body = RestIo.ReadBody<PlanLessonsRequestBody>(request);
            service.PlanLessons(
                body.FirstStartTime,
                body.FirstEndTime,
                body.ScheduleEndTime,
                body.PeriodInDays,
                body.AddressId,
                body.StudentIds);
            return string.Empty;
        });
    }
}
