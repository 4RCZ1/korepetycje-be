using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

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
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateTimetableServiceAsync(identity);
            var body = RestIo.ReadBody<DeleteLessonRequestBody>(request);
            service.DeleteLesson(body.LessonId, body.DeleteFutureLessons, role);
            return string.Empty;
        });
    }
}
