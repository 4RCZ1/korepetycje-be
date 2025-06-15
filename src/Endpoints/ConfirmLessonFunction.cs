using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints;

public class ConfirmLessonFunction
{
    public static Task<APIGatewayProxyResponse> ConfirmLesson(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var lessonExternalId = RestIo.GetPathParameter(request, "lessonId");
            const string studentExternalId = "1"; // todo: get from authentication token instead
            service.ConfirmLesson(lessonExternalId, studentExternalId);
            return string.Empty;
        });
    }
}
