using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints;

public class EditLessonFunction
{
    private class EditLessonRequestBody
    {
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public required bool EditFutureLessons { get; set; }
    }

    public static Task<APIGatewayProxyResponse> EditLesson(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var lessonExternalId = RestIo.GetPathParameter(request, "lessonId");
            var body = RestIo.ReadBody<EditLessonRequestBody>(request);
            service.EditLesson(
                lessonExternalId,
                body.StartTime,
                body.EndTime,
                body.EditFutureLessons);
            return string.Empty;
        });
    }
}
