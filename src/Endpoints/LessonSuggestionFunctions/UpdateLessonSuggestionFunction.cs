using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;

namespace Endpoints.LessonSuggestionFunctions;

public class UpdateLessonSuggestionFunction
{
    public static Task<APIGatewayProxyResponse> UpdateLessonSuggestion(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            var externalId = RestIo.GetPathParameter(request, "externalId");
            LessonSuggestionDto updatedLessonSuggestion = RestIo.ReadBody<LessonSuggestionDto>(request);
            service.UpdateLessonSuggestion(externalId, updatedLessonSuggestion);
            return "";
        });
    }
}