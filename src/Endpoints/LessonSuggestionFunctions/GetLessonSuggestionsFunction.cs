using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.LessonSuggestionFunctions;

public class GetLessonSuggestionsFunction
{
    public static Task<APIGatewayProxyResponse> GetLessonSuggestions(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var suggestedStart = RestIo.GetOptionalQueryParameter(request, "suggestedStart");
            var suggestedEnd = RestIo.GetOptionalQueryParameter(request, "suggestedEnd");
            var studentExternalId = RestIo.GetOptionalQueryParameter(request, "studentExternalId");
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            var lessonSuggestions = service.GetLessonSuggestion(
                suggestedStart, suggestedEnd, studentExternalId, role);
            return lessonSuggestions;
        });
    }
}
