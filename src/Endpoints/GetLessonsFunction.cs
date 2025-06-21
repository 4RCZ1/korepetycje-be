using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class GetLessonsFunction
{
    public static Task<APIGatewayProxyResponse> GetLessonsHandler(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var service = await ServiceFactory.CreateTimetableServiceAsync();
            if (identity.AsStudent.HasValue)
            {
                return service.GetLessonsAsStudent(
                    RestIo.GetQueryParameter(request, "startTime"),
                    RestIo.GetQueryParameter(request, "endTime"),
                    identity.RequireStudent());
            }

            var tutorRole = identity.RequireTutor();
            var studentExternalId = RestIo.GetOptionalQueryParameter(request, "studentId");
            if (studentExternalId is not null)
            {
                return service.GetStudentLessons(
                    studentExternalId,
                    RestIo.GetQueryParameter(request, "startTime"),
                    RestIo.GetQueryParameter(request, "endTime"),
                    tutorRole);
            }
            else
            {
                return service.GetLessons(
                    RestIo.GetQueryParameter(request, "startTime"),
                    RestIo.GetQueryParameter(request, "endTime"),
                    tutorRole);
            }
        });
    }
}
