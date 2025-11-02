using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.LessonSuggestionFunctions;

public class GetLessonSuggestionsFunction
{
    public static Task<APIGatewayProxyResponse> GetLessonSuggestions(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var suggestedStart = RestIo.GetOptionalQueryParameter(request, "suggestedStart");
            var suggestedEnd = RestIo.GetOptionalQueryParameter(request, "suggestedEnd");
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync(identity);
            if (identity.AsStudent.HasValue)
            {
                var role = identity.RequireStudent();
                return service.GetLessSuggsAsStudent(suggestedStart, suggestedEnd, role);
            }
            else
            {
                var role = identity.RequireTutor();
                var studentExternalId = RestIo.GetOptionalQueryParameter(request, "studentExternalId");
                return service.GetLessonSuggestion(
                    suggestedStart, suggestedEnd, studentExternalId, role);
            }
        });
    }
}
