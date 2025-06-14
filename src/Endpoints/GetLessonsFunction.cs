using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Endpoints;

public class GetLessonsFunction
{
    public static Task<APIGatewayProxyResponse> GetLessonsHandler(
        APIGatewayProxyRequest request, ILambdaContext context)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            if (request.QueryStringParameters.TryGetValue(
                    "studentId",
                    out var studentExternalId))
            {
                return service.GetStudentLessons(
                    studentExternalId,
                    request.QueryStringParameters["startTime"],
                    request.QueryStringParameters["endTime"]);
            }
            else
            {
                return service.GetLessons(
                    request.QueryStringParameters["startTime"],
                    request.QueryStringParameters["endTime"]);
            }
        });
    }
}
