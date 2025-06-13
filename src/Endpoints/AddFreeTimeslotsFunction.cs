using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class AddFreeTimeslotsFunction
{
    private class RequestBody
    {
        public required string StartTime { get; set; }
        public required string EndTime { get; set; }
        public required int? PeriodInDays { get; set; }
        public required int DurationInMinutes { get; set; }
    }

    public async Task<APIGatewayProxyResponse> AddFreeTimeslots(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableServiceAsync();
        var body = JsonSerializer.Deserialize<RequestBody>(request.Body);
        service.AddFreeTerm(body.StartTime, body.EndTime); // todo: implement time series
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
        };
    }
}
