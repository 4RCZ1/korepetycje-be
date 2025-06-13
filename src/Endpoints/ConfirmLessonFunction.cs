using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class ConfirmLessonFunction
{
    public async Task<APIGatewayProxyResponse> ConfirmLesson(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableServiceAsync();
        var lessonExternalId = request.PathParameters["lessonExternalId"];
        const string studentExternalId = "1"; // todo: get from authentication token instead
        service.ConfirmLesson(lessonExternalId, studentExternalId);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}
