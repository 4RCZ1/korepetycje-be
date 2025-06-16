using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Endpoints.Interfaces;

namespace Endpoints.LessonSuggestionFunctions;

public class AddLessonSuggestionFunction
{
    public static Task<APIGatewayProxyResponse> AddLessonSuggestion(APIGatewayProxyRequest request)
    {
        return RestIo.HandleRestExceptionsAsync(async () =>
        {
            var service = await ServiceFactory.CreateLessonSuggestionServiceAsync();
            var body = RestIo.ReadBody<LessonSuggestionDto>(request);
            var message = service.AddLessonSuggestion(body);
            return message;
        });
    }
}