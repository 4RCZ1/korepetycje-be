using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class EditLessonDetailsFunction
{
    private class EditLessonDetailsRequestBody
    {
        public required string Description { get; set; }
    }

    public static async Task<APIGatewayProxyResponse> EditLessonDetails(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var externalLessonId = RestIo.GetPathParameter(request, "lessonId");
            var body = RestIo.ReadBody<EditLessonDetailsRequestBody>(request);
            var service = await ServiceFactory.CreateTimetableServiceAsync(identity);
            service.EditLessonDetails(externalLessonId, body.Description, role);
            return string.Empty;
        });
    }
}
