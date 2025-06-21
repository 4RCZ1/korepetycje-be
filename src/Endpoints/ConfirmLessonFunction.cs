using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class ConfirmLessonFunction
{
    private class ConfirmLessonRequestBody
    {
        public required bool Confirmed { get; set; }
    }

    public static Task<APIGatewayProxyResponse> ConfirmLesson(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireStudent();
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var lessonExternalId = RestIo.GetPathParameter(request, "lessonId");
            var body = RestIo.ReadBody<ConfirmLessonRequestBody>(request);
            service.ConfirmLesson(body.Confirmed, lessonExternalId, role);
            return string.Empty;
        });
    }
}
