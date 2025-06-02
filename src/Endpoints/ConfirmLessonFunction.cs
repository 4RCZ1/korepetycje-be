using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class ConfirmLessonFunction
{
    public async Task<APIGatewayProxyResponse> ConfirmLesson(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        var lessonUuid = request.PathParameters["lessonUuid"];
        service.ConfirmLesson(lessonUuid);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            },
        };
    }
}
