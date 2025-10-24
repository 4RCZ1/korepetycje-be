using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Dto;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.LessonSuggestionFunctions;

public class UpdateLessonSuggestionFunction
{
    public static Task<APIGatewayProxyResponse> UpdateLessonSuggestion(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            var externalId = RestIo.GetPathParameter(request, "suggestionId");
            LessonSuggestionDto updatedLessonSuggestion = RestIo.ReadBody<LessonSuggestionDto>(request);
            service.UpdateLessonSuggestion(externalId, updatedLessonSuggestion, role);
            return "";
        });
    }
}
