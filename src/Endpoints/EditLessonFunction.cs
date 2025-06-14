using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class EditLessonFunction
{
    private class RequestBody
    {
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public required bool EditFutureLessons { get; set; }
    }

    public static Task<APIGatewayProxyResponse> EditLesson(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var lessonExternalId = RestIo.GetPathParameter(request, "lessonExternalId");
            var body = RestIo.ReadBody<RequestBody>(request);
            service.EditLesson(
                lessonExternalId,
                body.StartTime,
                body.EndTime,
                body.EditFutureLessons);
            return string.Empty;
        });
    }
}
