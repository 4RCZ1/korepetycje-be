using Amazon.Lambda.APIGatewayEvents;

namespace Endpoints;

public class GetLessonsFunction
{
    public static Task<APIGatewayProxyResponse> GetLessonsHandler(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            var studentExternalId = RestIo.GetOptionalQueryParameter(request, "studentId");
            if (studentExternalId is not null)
            {
                return service.GetStudentLessons(
                    studentExternalId,
                    RestIo.GetQueryParameter(request, "startTime"),
                    RestIo.GetQueryParameter(request, "endTime"));
            }
            else
            {
                return service.GetLessons(
                    RestIo.GetQueryParameter(request, "startTime"),
                    RestIo.GetQueryParameter(request, "endTime"));
            }
        });
    }
}
