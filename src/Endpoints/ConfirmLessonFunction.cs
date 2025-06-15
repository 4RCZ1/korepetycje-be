using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints;

public class ConfirmLessonFunction
{
    private class ConfirmLessonRequestBody
    {
        public required bool Confirmed { get; set; }
    }

    public static Task<APIGatewayProxyResponse> ConfirmLesson(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var lessonExternalId = RestIo.GetPathParameter(request, "lessonId");
            var body = RestIo.ReadBody<ConfirmLessonRequestBody>(request);
            const string studentExternalId = "1"; // todo: get from authentication token instead
            service.ConfirmLesson(body.Confirmed, lessonExternalId, studentExternalId);
            return string.Empty;
        });
    }
}
