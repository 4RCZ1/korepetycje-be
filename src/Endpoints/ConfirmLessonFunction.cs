using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class ConfirmLessonFunction
{
    public async Task<APIGatewayProxyResponse> ConfirmLesson(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        var lessonExternalId = request.PathParameters["lessonExternalId"];
        service.ConfirmLesson(lessonExternalId);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}
