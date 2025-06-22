using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.LessonSuggestionFunctions;

public class GetLessSuggsAsStudentFunction
{
    public static Task<APIGatewayProxyResponse> GetLessSuggsAsStudent(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            if (identity.AsStudent.HasValue)
            {
                return service.GetLessSuggsAsStudent(
                    RestIo.GetOptionalQueryParameter(request, "startTime"),
                    RestIo.GetOptionalQueryParameter(request, "endTime"),
                    identity.RequireStudent());
            }
            else
            {
                throw new ApplicationException("Student identity is null");
            }
        });
    }
}