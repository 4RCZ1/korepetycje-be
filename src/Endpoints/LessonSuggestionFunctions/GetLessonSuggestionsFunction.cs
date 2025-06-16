using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;

namespace Endpoints.LessonSuggestionFunctions;

public class GetLessonSuggestionsFunction
{
    public static Task<APIGatewayProxyResponse> GetLessonSuggestions(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            string? suggestedStart = null;
            string? suggestedEnd = null;
            string? studentExternalId = null;
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            request.QueryStringParameters?.TryGetValue("suggestedStart", out suggestedStart);
            request.QueryStringParameters?.TryGetValue("suggestedEnd", out suggestedEnd);
            request.QueryStringParameters?.TryGetValue("studentExternalId", out studentExternalId);
            var lessonSuggestions = service.GetLessonSuggestion(suggestedStart, suggestedEnd, studentExternalId);
            return lessonSuggestions;
        });
    }
}