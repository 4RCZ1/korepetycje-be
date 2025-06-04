using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class AddFreeTerm
{
    public async Task<APIGatewayProxyResponse> ConfirmLesson(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableService();
        var start = request.QueryStringParameters["startTime"];
        var end = request.QueryStringParameters["endTime"];
        service.AddFreeTerm(start, end);
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}