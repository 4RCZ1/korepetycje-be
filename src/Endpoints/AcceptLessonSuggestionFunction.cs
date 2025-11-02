using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces.Authorization;

namespace Endpoints;

public class AcceptLessonSuggestionFunction
{
    private class AcceptLessonSuggestionBody
    {
        public required bool Accept { get; set; }
    }

    public static async Task<APIGatewayProxyResponse> AcceptLessonSuggestion(
        APIGatewayProxyRequest request)
    {
        return await RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireStudent();
            var suggestionId = RestIo.GetPathParameter(request, "suggestionId");
            var body = RestIo.ReadBody<AcceptLessonSuggestionBody>(request);
            var service = await ServiceFactory.CreateTimetableServiceAsync(identity);
            service.AcceptSuggestion(suggestionId, body.Accept, role);
            return string.Empty;
        });
    }
}
