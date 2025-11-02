using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class PlanLessonsFunction
{
    private class PlanLessonsRequestBody
    {
        public required DateTimeOffset FirstStartTime { get; set; }
        public required DateTimeOffset FirstEndTime { get; set; }
        public required DateTimeOffset ScheduleEndTime { get; set; }
        public required int PeriodInDays { get; set; }
        public required string AddressId { get; set; }
        public required IList<string> StudentIds { get; set; }
    }

    public static Task<APIGatewayProxyResponse> PlanLessons(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateTimetableServiceAsync(identity);
            var body = RestIo.ReadBody<PlanLessonsRequestBody>(request);
            service.PlanLessons(
                body.FirstStartTime,
                body.FirstEndTime,
                body.ScheduleEndTime,
                body.PeriodInDays,
                body.AddressId,
                body.StudentIds,
                role);
            return string.Empty;
        });
    }
}
