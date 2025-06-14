using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

[assembly: LambdaSerializer(typeof(CamelCaseLambdaJsonSerializer))]

namespace Endpoints;

public class GetLessonsFunction
{
    public static async Task<APIGatewayProxyResponse> GetLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        var service = await ServiceFactory.CreateTimetableServiceAsync();
        if (request.QueryStringParameters.TryGetValue("studentExternalId", out var studentExternalId))
        {
            return RestIo.OkJson(service.GetStudentLessons(
                studentExternalId,
                request.QueryStringParameters["startTime"],
                request.QueryStringParameters["endTime"]));
        }
        else
        {
            return RestIo.OkJson(service.GetLessons(
                request.QueryStringParameters["startTime"],
                request.QueryStringParameters["endTime"]));
        }
    }
}
