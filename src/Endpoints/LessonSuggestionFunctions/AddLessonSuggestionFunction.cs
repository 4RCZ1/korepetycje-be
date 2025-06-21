using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;
using Endpoints.Interfaces.Authorization;

namespace Endpoints.LessonSuggestionFunctions;

public class AddLessonSuggestionFunction
{
    public static Task<APIGatewayProxyResponse> AddLessonSuggestion(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestBoilerplateAsync(request, async identity =>
        {
            var role = identity.RequireTutor();
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            var body = RestIo.ReadBody<LessonSuggestionDto>(request);
            var message = service.AddLessonSuggestion(body, role);
            return message;
        });
    }
}
