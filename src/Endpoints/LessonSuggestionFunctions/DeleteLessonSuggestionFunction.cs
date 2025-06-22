using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.LessonSuggestionFunctions;

public class DeleteLessonSuggestionFunction
{
    public static Task<APIGatewayProxyResponse> DeleteLessonSuggestion(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            var externalId = RestIo.GetPathParameter(request, "suggestionId");
            service.DeleteLessonSuggestion(externalId, role);
            return "";
        });
    }
}
