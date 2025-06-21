using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints;

public class DeleteLessonFunction
{
    private class DeleteLessonRequestBody
    {
        public required string LessonId { get; set; }
        public required bool DeleteFutureLessons { get; set; }
    }

    public static Task<APIGatewayProxyResponse> DeleteLesson(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var body = RestIo.ReadBody<DeleteLessonRequestBody>(request);
            service.DeleteLesson(body.LessonId, body.DeleteFutureLessons);
            return string.Empty;
        });
    }
}
