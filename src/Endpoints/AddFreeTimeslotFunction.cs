using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class AddFreeTimeslotFunction
{
    public async Task<APIGatewayProxyResponse> AddFreeTimeslot(
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